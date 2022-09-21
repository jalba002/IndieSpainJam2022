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

        void OnAltFire()
        {
            Shoot();
        }
        
        void Shoot()
        {
            spellTest.Cast(spawnPoint.position, transform.forward, transform.rotation, new AttributesData(10f, 5f));   
        }
    }
}