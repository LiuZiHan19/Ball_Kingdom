using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class EntityPrefabManager : MonoBehaviour
{
    public static EntityPrefabManager Instance { get; private set; }

    public Dictionary<string, Entity> Prefabs = new Dictionary<string, Entity>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("单例重复实例化EntityPrefabProvider");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<PrefabIdentifyComponent>()
            .Build(entityManager);
        var prefabIdentifyArray = entityQuery.ToComponentDataArray<PrefabIdentifyComponent>(Allocator.Temp);
        var entities = entityQuery.ToEntityArray(Allocator.Temp);
        for (int i = 0; i < prefabIdentifyArray.Length; i++)
        {
            Prefabs.Add(prefabIdentifyArray[i].Identify.ToString(), entities[i]);
        }
    }
}