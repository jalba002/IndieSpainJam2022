using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(LaserSpellWeak), menuName = "CosmosDefender/" + nameof(LaserSpellWeak))]
    public class LaserSpellWeak : BaseSpell
    {
        protected override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData, SpellTester caster)
        {
            var instance = Instantiate(prefab, spawnPoint, rotation);
            instance.InstantiateBullet(spawnPoint, forward, rotation, combatData, currentData);
        }
    }
}