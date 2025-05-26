using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

[BurstCompile]
public partial struct CollisionBorderSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var simSingleton = SystemAPI.GetSingleton<SimulationSingleton>();

        var collisionResponseJob = new CollisionBorderJob
        {
            CharacterMoveLookup = SystemAPI.GetComponentLookup<CharacterMove>(),
            BorderTagLookup = SystemAPI.GetComponentLookup<BorderTag>(),
            VelocityLookup = SystemAPI.GetComponentLookup<PhysicsVelocity>()
        };

        state.Dependency = collisionResponseJob.Schedule(simSingleton, state.Dependency);
    }
}

[BurstCompile]
public struct CollisionBorderJob : ICollisionEventsJob
{
    public ComponentLookup<CharacterMove> CharacterMoveLookup;
    public ComponentLookup<BorderTag> BorderTagLookup;
    public ComponentLookup<PhysicsVelocity> VelocityLookup;

    [BurstCompile]
    public void Execute(CollisionEvent collisionEvent)
    {
        Entity character;

        if (CharacterMoveLookup.HasComponent(collisionEvent.EntityA) &&
            BorderTagLookup.HasComponent(collisionEvent.EntityB))
        {
            character = collisionEvent.EntityA;
        }
        else if (CharacterMoveLookup.HasComponent(collisionEvent.EntityB) &&
                 BorderTagLookup.HasComponent(collisionEvent.EntityA))
        {
            character = collisionEvent.EntityB;
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