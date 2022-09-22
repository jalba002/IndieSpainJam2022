using UnityEngine;

namespace CosmosDefender.Bullets
{
    public class BulletSpawnManager : MonoBehaviour
    {
        public static void Spawn(GameObject projectilePrefab, Vector3 position, Quaternion identity)
        {
            var proj = Instantiate(projectilePrefab, position, identity);
            proj.name = "Projectile";
            proj.GetComponent<BaseBullet>().InstantiateBullet();
        }
    }
}