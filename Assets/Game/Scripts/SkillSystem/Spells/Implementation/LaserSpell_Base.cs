using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(LaserSpell_Base), menuName = "CosmosDefender/" + nameof(LaserSpell_Base))]
    public class LaserSpell_Base : BaseSpell
    {
        public override void Cast(Transform spawnPoint, IReadOnlyOffensiveData combatData)
        {
            // This spell casts lasers around the player.
            
        }

        public override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData)
        {
            var instance = Object.Instantiate(prefab, spawnPoint, rotation);
            instance.GetComponent<Bullet>().InstantiateBullet();
        }
    }
}