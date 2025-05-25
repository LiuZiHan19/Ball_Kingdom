using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class CharacterMoveAuthoring : MonoBehaviour
{
    public float Speed;
    public float3 Direction;
    public float3 AngularVelocity;

    public class Baker : Baker<CharacterMoveAuthoring>
    {
        public override void Bake(CharacterMoveAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CharacterMove()
            {
                Speed = authoring.Speed,
                Direction = authoring.Direction,
                AngularVelocity = authoring.AngularVelocity
            });
        }
    }
}

public struct CharacterMove : IComponentData
{
    public float Speed;
    public float3 Direction;
    public float3 AngularVelocity;
}