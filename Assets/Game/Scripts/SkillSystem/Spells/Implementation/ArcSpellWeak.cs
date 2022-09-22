using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(ArcSpellWeak), menuName = "CosmosDefender/" + nameof(ArcSpellWeak))]
    public class ArcSpellWeak : BaseSpell
    {
        protected override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData)
        {
            var instance = Instantiate(prefab, spawnPoint, rotation);
            instance.InstantiateBullet(spawnPoint, forward, rotation, combatData, currentData);
        }
    }
}