using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text leftGenerationNumber;
    [SerializeField] private Text rightGenerationNumber;

    private EntityManager _entityManager;

    private void Awake()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void Update()
    {
        EntityQuery spawnerQuery =
            new EntityQueryBuilder(Allocator.Temp).WithAll<CharacterSpawner>().Build(_entityManager);

        var spawners = spawnerQuery.ToComponentDataArray<CharacterSpawner>(Allocator.Temp);
        foreach (var spawner in spawners)
        {
            if (spawner.Type == CharacterType.Left)
            {
                var number = (spawner.SpawnWaveCount * spawner.SpawnCount).ToString();
                if (leftGenerationNumber.text != number)
                {
                    leftGenerationNumber.text = number;
                }
            }
            else
            {
                var number = (spawner.SpawnWaveCount * spawner.SpawnCount).ToString();
                if (rightGenerationNumber.text != number)
                {
                    rightGenerationNumber.text = number;
                }
            }
        }

        spawners.Dispose();
        spawnerQuery.Dispose();
    }
}