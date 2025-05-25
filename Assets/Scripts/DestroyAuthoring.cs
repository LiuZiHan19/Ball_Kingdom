using Unity.Entities;
using UnityEngine;

public class DestroyAuthoring : MonoBehaviour
{
    public class Baker : Baker<DestroyAuthoring>
    {
        public override void Bake(DestroyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent<DestroyTag>(entity);
            SetComponentEnabled<DestroyTag>(entity, false);
        }
    }
}

public struct DestroyTag : IComponentData, IEnableableComponent
{
}