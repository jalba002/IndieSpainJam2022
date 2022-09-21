using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(ThunderSpell), menuName = "CosmosDefender/" + nameof(ThunderSpell))]
    public class ThunderSpell : BaseSpell
    {
        [SerializeField]
        private int amountOfMissiles;

        public override void Cast(Transform spawnPoint, IReadOnlyCombatData combatData)
        {
            // 
        }
        
        public override void Cast(Transform spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyCombatData combatData)
        {
            var projectile = Object.Instantiate(prefab, spawnPoint.position, rotation);
            projectile.GetComponent<Bullet>().InstantiateBullet(spawnPoint, forward, rotation);
        }
    }
}