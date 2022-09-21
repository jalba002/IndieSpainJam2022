using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(LaserSpellWeak), menuName = "CosmosDefender/" + nameof(LaserSpellWeak))]
    public class LaserSpellWeak : BaseSpell
    {
        public override void Cast(Transform spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData)
        {
            var instance = Instantiate(prefab, spawnPoint.position, rotation);
            instance.InstantiateBullet(spawnPoint, forward, rotation);
        }
    }
}