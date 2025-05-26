using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
[BurstCompile]
public partial struct CharacterSpawnerSystem : ISystem
{
    private Random _random;

    public void OnCreate(ref SystemState state)
    {
        _random = new Random((uint)System.DateTime.Now.Ticks);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        var characterSpawnerJob = new CharacterSpawnerJob
        {
            ecb = ecb.AsParallelWriter(),
            RandomSeed = _random.NextUInt(),
            ElapsedTime = SystemAPI.Time.ElapsedTime
        };

        state.Dependency = characterSpawnerJob.ScheduleParallel(state.Dependency);
    }
}

[BurstCompile]
public partial struct CharacterSpawnerJob : IJobEntity
{
    [NativeDisableParallelForRestriction] public EntityCommandBuffer.ParallelWriter ecb;
    public double ElapsedTime;
    public uint RandomSeed;

    [BurstCompile]
    public void Execute([ChunkIndexInQuery] int chunkIndex, ref CharacterSpawner spawner)
    {
        if (spawner.SpawnCompleted)
        {
            return;
        }

        if (ElapsedTime < spawner.ElapsedTime)
        {
            return;
        }

        var random = new Random(RandomSeed + (uint)chunkIndex);

        for (int i = 0; i < spawner.SpawnCount; i++)
        {
            var instance = ecb.Instantiate(chunkIndex, spawner.SpawnPrefab);
            var zOffset = random.NextFloat(-spawner.SpawnZOffset, spawner.SpawnZOffset);

            ecb.SetComponent(chunkIndex, instance, new LocalTransform
            {
                Position = spawner.SpawnPosition + new float3(0, 0, zOffset),
                Rotation = quaternion.identity,
                Scale = 0.3f
            });

            ecb.SetComponent(chunkIndex, instance, new CharacterMove()
            {
                Speed = 1.75f,
                Direction = new float3(spawner.Type == CharacterType.Left ? 1 : -1, 0,
                    spawner.Type == CharacterType.Left ? 1f : 1f),
                AngularVelocity = new float3(0, random.NextFloat(5, 8), 0)
            });
        }

        spawner.ElapsedTime = ElapsedTime + spawner.SpawnCooldownTime;

        spawner.SpawnWaveCount++;
        if (spawner.SpawnWaveCount == spawner.MaxWaveCount)
        {
            spawner.SpawnCompleted = true;
        }
    }
}