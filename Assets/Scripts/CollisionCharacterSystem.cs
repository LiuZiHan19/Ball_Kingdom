using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

[BurstCompile]
public partial struct CollisionCharacterSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var simSingleton = SystemAPI.GetSingleton<SimulationSingleton>();

        var collisionCharacterJob = new CollisionCharacterJob
        {
            CharacterMoveLookup = SystemAPI.GetComponentLookup<CharacterMove>(),
            VelocityLookup = SystemAPI.GetComponentLookup<PhysicsVelocity>()
        };

        state.Dependency = collisionCharacterJob.Schedule(simSingleton, state.Dependency);
    }
}

[BurstCompile]
public struct CollisionCharacterJob : ICollisionEventsJob
{
    public ComponentLookup<CharacterMove> CharacterMoveLookup;
    public ComponentLookup<PhysicsVelocity> VelocityLookup;

    [BurstCompile]
    public void Execute(CollisionEvent collisionEvent)
    {
        Entity character;

        if (CharacterMoveLookup.HasComponent(collisionEvent.EntityA) &&
            CharacterMoveLookup.HasComponent(collisionEvent.EntityB))
        {
            character = collisionEvent.EntityA;
        }
        else
        {
            return;
        }

        var contactNormal = collisionEvent.Normal;
        var velocity = VelocityLookup[character].Linear;
        var direction = math.normalizesafe(velocity);

        var reflectedDirection = math.reflect(direction, contactNormal);
        reflectedDirection.y = 0;

        var characterMove = CharacterMoveLookup[character];
        characterMove.Direction = math.normalizesafe(reflectedDirection);
        CharacterMoveLookup[character] = characterMove;
    }
}