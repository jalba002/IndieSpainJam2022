using UnityEngine;
using Bullets;
using CosmosDefender;

namespace Player
{
    public class PlayerProjectiles : MonoBehaviour
    {
        public Transform spawnPoint;
        public Vector3 size;

        public GameObject testPrefab;

        public BaseSpell spellTest;

        [SerializeField]
        private PlayerAttributes playerAttributes;

        void OnAltFire()
        {
            Shoot();
        }
        
        void Shoot()
        {
            spellTest.Cast(spawnPoint, transform.forward, transform.rotation, playerAttributes.CombatData);
        }
    }
}