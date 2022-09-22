using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(ThunderSpell), menuName = "CosmosDefender/" + nameof(ThunderSpell))]
    public class ThunderSpell : BaseSpell
    {
        [SerializeField]
        private int amountOfMissiles;

        public override void Cast(Transform spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData)
        {
            var projectile = Instantiate(prefab, spawnPoint.position, rotation);
            projectile.InstantiateBullet(spawnPoint, forward, rotation, combatData, currentData);
        }
    }
}