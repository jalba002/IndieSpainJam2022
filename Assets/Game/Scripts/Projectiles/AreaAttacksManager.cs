using UnityEngine;

namespace CosmosDefender
{
    public static class AreaAttacksManager
    {
        // This class shows Physics.Overlaps debugs.
        public static Collider[] BoxAttack(Vector3 spawnPos, Vector3 halfSize, Quaternion spawnRot,
            float debugDuration = 0.1f)
        {
            var a = Physics.OverlapBox(spawnPos, halfSize, spawnRot);

            ExtDebug.DrawBox(spawnPos, halfSize, spawnRot, Color.red, debugDuration);

            return a;
        }

        public static Collider[] BoxAttack(Vector3 spawnPos, Vector3 forward, Vector3 halfSize, Quaternion spawnRot,
            float debugDuration = 0.1f)
        {
            Vector3 l_SpawnPos = spawnPos + (forward * halfSize.z);
            var a = Physics.OverlapBox(l_SpawnPos, halfSize, spawnRot);

            ExtDebug.DrawBox(l_SpawnPos, halfSize, spawnRot, Color.red, debugDuration);

            return a;
        }

        public static Collider[] SphereOverlap(Vector3 spawnPos, float radius, bool debugMode = true)
        {
            var a = Physics.OverlapSphere(spawnPos, radius);

            if (debugMode)
                ExtDebug.DrawSphere(spawnPos, radius, Color.red);

            return a;
        }

        // Manage all hits here.
        public static void DealDamageToCollisions<T>(Collider[] colliders, float damage) where T : IDamageable
        {
            foreach (var item in colliders)
            {
                item.GetComponent<T>()?.DealDamage(damage);
            }
        }
    }
}