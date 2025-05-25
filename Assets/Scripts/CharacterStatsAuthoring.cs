using Unity.Entities;
using UnityEngine;

public class CharacterStatsAuthoring : MonoBehaviour
{
    public CharacterType Type;
    public int CurrentHealth;
    public int Damage;
    public double CooldownTime;

    public class Baker : Baker<CharacterStatsAuthoring>
    {
        public override void Bake(CharacterStatsAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CharacterStats()
            {
                Type = authoring.Type,
                CurrentHealth = authoring.CurrentHealth,
                Damage = authoring.Damage,
                CooldownTime = authoring.CooldownTime
            });
        }
    }
}

public struct CharacterStats : IComponentData
{
    public CharacterType Type;
    public int CurrentHealth;
    public int Damage;
    public double CooldownTime;
    public double ElapsedTime;
}