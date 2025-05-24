using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class CharacterAuthoring : MonoBehaviour
{
    public float VelocityFactor;

    public class Baker : Baker<CharacterAuthoring>
    {
        public override void Bake(CharacterAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CharacterVelocity()
            {
                Value = authoring.VelocityFactor
            });

            AddComponent(entity, new CharacterEntity()
            {
            });
        }
    }
}

public struct CharacterEntity : IComponentData
{
    public Entity Value;
}

public struct CharacterVelocity : IComponentData
{
    public float Value;
}

public struct InitializeCharacterFlag : IComponentData, IEnableableComponent
{
}

public partial struct CharacterVelocitySystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (velocity, factor) in SystemAPI
                     .Query<RefRW<PhysicsVelocity>, RefRW<CharacterVelocity>>())
        {
            velocity.ValueRW.Linear *= factor.ValueRW.Value;
        }
    }
}

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct InitializationCharacterSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (velocity, shouldInitialize) in SystemAPI
                     .Query<PhysicsVelocity, EnabledRefRW<InitializeCharacterFlag>>())
        {
        }
    }
}