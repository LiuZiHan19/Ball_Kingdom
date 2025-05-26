using Unity.Entities;
using UnityEngine;

public class BorderAuthoring : MonoBehaviour
{
    public class Baker : Baker<BorderAuthoring>
    {
        public override void Bake(BorderAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new BorderTag());
        }
    }
}

public struct BorderTag : IComponentData
{
}