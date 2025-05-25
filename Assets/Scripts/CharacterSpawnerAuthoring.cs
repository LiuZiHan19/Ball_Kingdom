using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class CharacterSpawnerAuthoring : MonoBehaviour
{
    public CharacterType Type;
    public GameObject SpawnPrefab;
    public int SpawnCount;
    public int WaveCount;
    public Vector3 SpawnPosition;
    public float SpawnZOffset;
    public double SpawnCooldownTime;

    public class Baker : Baker<CharacterSpawnerAuthoring>
    {
        public override void Bake(CharacterSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CharacterSpawner()
            {
                Type = authoring.Type,
                SpawnPrefab = GetEntity(authoring.SpawnPrefab, TransformUsageFlags.Dynamic),
                SpawnCount = authoring.SpawnCount,
                MaxWaveCount = authoring.WaveCount,
                SpawnPosition = authoring.SpawnPosition,
                SpawnZOffset = authoring.SpawnZOffset,
                SpawnCooldownTime = authoring.SpawnCooldownTime,
            });
        }
    }
}

public struct CharacterSpawner : IComponentData
{
    public CharacterType Type;
    public Entity SpawnPrefab;
    public int SpawnCount;
    public int MaxWaveCount;
    public float3 SpawnPosition;
    public float SpawnZOffset;
    public bool SpawnCompleted;
    public double SpawnCooldownTime;
    public double ElapsedTime;
    public int SpawnWaveCount;
}

public enum CharacterType : byte
{
    Left,
    Right
}