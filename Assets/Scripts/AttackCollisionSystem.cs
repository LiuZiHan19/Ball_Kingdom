using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

[BurstCompile]
public partial struct AttackCollisionSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var simSingleton = SystemAPI.GetSingleton<SimulationSingleton>();

        AttackCollisionJob attackCollisionJob = new AttackCollisionJob()
        {
            CharacterStatsLookup = SystemAPI.GetComponentLookup<CharacterStats>(),
            DestroyTagLookup = SystemAPI.GetComponentLookup<DestroyTag>(),
            ElapsedTime = SystemAPI.Time.ElapsedTime
        };

        state.Dependency = attackCollisionJob.Schedule(simSingleton, state.Dependency);
    }
}

[BurstCompile]
public struct AttackCollisionJob : ICollisionEventsJob
{
    public ComponentLookup<CharacterStats> CharacterStatsLookup;
    public ComponentLookup<DestroyTag> DestroyTagLookup;
    public double ElapsedTime;

    [BurstCompile]
    public void Execute(CollisionEvent collisionEvent)
    {
        Entity character1;
        Entity character2;

        if (CharacterStatsLookup.HasComponent(collisionEvent.EntityA) &&
            CharacterStatsLookup.HasComponent(collisionEvent.EntityB))
        {
            character1 = collisionEvent.EntityA;
            character2 = collisionEvent.EntityB;
        }
        else if (CharacterStatsLookup.HasComponent(collisionEvent.EntityB) &&
                 CharacterStatsLookup.HasComponent(collisionEvent.EntityA))
        {
            character1 = collisionEvent.EntityB;
            character2 = collisionEvent.EntityA;
        }
        else
        {
            return;
        }

        var stats1 = CharacterStatsLookup[character1];
        var stats2 = CharacterStatsLookup[character2];

        // Ignore same type collision
        if (stats1.Type == stats2.Type)
        {
            return;
        }

        if (ElapsedTime < stats2.ElapsedTime)
        {
            return;
        }

        stats1.CurrentHealth -= stats2.Damage;
        stats2.CurrentHealth -= stats1.Damage;

        CharacterStatsLookup[character1] = stats1;
        CharacterStatsLookup[character2] = stats2;

        stats1.ElapsedTime = ElapsedTime + stats1.CooldownTime;
        stats2.ElapsedTime = ElapsedTime + stats2.CooldownTime;

        CharacterStatsLookup[character1] = stats1;
        CharacterStatsLookup[character2] = stats2;

        if (stats1.CurrentHealth <= 0)
        {
            DestroyTagLookup.SetComponentEnabled(character1, true);
        }

        if (stats2.CurrentHealth <= 0)
        {
            DestroyTagLookup.SetComponentEnabled(character2, true);
        }
    }
}