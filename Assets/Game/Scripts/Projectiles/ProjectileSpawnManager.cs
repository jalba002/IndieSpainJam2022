using UnityEngine;

namespace Bullets
{
    public class BulletSpawnManager : MonoBehaviour
    {
        public static void Spawn(GameObject projectilePrefab, Vector3 position, Quaternion identity)
        {
            var proj = Instantiate(projectilePrefab, position, identity);
            proj.name = "Projectile";
        }
    }
}