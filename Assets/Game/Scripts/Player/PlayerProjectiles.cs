using UnityEngine;
using Bullets;

namespace Player
{
    public class PlayerProjectiles : MonoBehaviour
    {
        public Transform spawnPoint;
        public Vector3 size;

        public GameObject testPrefab;

        void OnAltFire()
        {
            //BulletSpawnManager.Spawn(testPrefab, spawnPoint.position, transform.rotation);
            
            // This should be on the bullet itself.
            // var colls = AreaAttacksManager.BoxAttack(spawnPoint.position, transform.forward, size * 0.5f, transform.rotation);
            
            // foreach (var VARIABLE in colls)
            // {
            //     Debug.Log(VARIABLE.name);
            // }
            
            
        }
    }
}