using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
public partial struct CharacterMoveSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        CharacterMoveJob moveJob = new CharacterMoveJob() { };
        moveJob.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct CharacterMoveJob : IJobEntity
{
    [BurstCompile]
    public void Execute(ref CharacterMove move, ref PhysicsVelocity velocity, ref LocalTransform transform)
    {
        float3 v = move.Speed * move.Direction;
        velocity.Linear = v;

        float3 angular = move.AngularVelocity;
        velocity.Angular = angular;

        // 固定y轴位置
        transform.Position.y = 0.15f;

        // 固定xz轴旋转
        quaternion q = transform.Rotation;
        q.value.x = 0;
        q.value.z = 0;
        transform.Rotation = q;
    }
}