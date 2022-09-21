using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(ThunderSpell), menuName = "CosmosDefender/" + nameof(ThunderSpell))]
    public class ThunderSpell : BaseSpell
    {
        [SerializeField]
        private int amountOfMissiles;

        public override void Cast(Transform spawnPoint, IReadOnlyOffensiveData combatData)
        {
            // 
        }

        public override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData)
        {
            var projectile = Object.Instantiate(prefab,spawnPoint, rotation);
            projectile.GetComponent<Bullet>().InstantiateBullet(spawnPoint, forward, rotation);
        }
    }
}