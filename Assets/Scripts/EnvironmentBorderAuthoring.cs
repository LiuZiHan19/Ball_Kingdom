using Unity.Entities;
using UnityEngine;

public class EnvironmentBorderAuthoring : MonoBehaviour
{
    public class Baker : Baker<EnvironmentBorderAuthoring>
    {
        public override void Bake(EnvironmentBorderAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new BorderTag());
        }
    }
}

public struct BorderTag : IComponentData
{
}