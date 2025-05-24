using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class PrefabIdentifyAuthoring : MonoBehaviour
{
    public GameObject Prefab;
    public string Identify;

    public class Baker : Baker<PrefabIdentifyAuthoring>
    {
        public override void Bake(PrefabIdentifyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PrefabIdentifyComponent()
            {
                Identify = authoring.Identify
            });
        }
    }
}

public struct PrefabIdentifyComponent : IComponentData
{
    public FixedString64Bytes Identify;
}