using Unity.Entities;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float amountOfCharacter;
    [SerializeField] private Entity characterPrefab;

    private void Start()
    {
        for (int i = 0; i < amountOfCharacter; i++)
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            entityManager.Instantiate(EntityPrefabManager.Instance.Prefabs["6da36343a1e8d8e4e8f1c10bf60043c2"]);
        }
    }
}