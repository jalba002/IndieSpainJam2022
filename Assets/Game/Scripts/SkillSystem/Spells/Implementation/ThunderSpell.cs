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

        public override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyCombatData combatData)
        {
            var projectile = Object.Instantiate(prefab,spawnPoint, rotation);
            projectile.transform.forward = forward;
            projectile.GetComponent<Projectile>();
        }
    }
}