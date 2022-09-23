using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CosmosDefender.Bullets.Implementation
{
    public class DashBullet : BaseBullet
    {
        private IReadOnlyOffensiveData combatData;
        private SpellData spellData;
        private Vector3 origin;
        private Transform firePoint;
        private PlayerMovementController playerMovement;
        private List<Collider> enemiesAlreadyHit = new List<Collider>();

        public override void InstantiateBullet(Vector3 origin, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData, SpellData spellData)
        {
            this.spellData = spellData;
            this.combatData = combatData;
            Destroy(this.gameObject, spellData.Lifetime);
        }

        void FixedUpdate()
        {
            CastRayDamage(transform.position, spellData.ProjectileRadius, raycastHitLayers);
        }

        private void CastRayDamage(Vector3 origin, float radius, LayerMask layerMask)
        {
            var enemyHit = AreaAttacksManager.SphereOverlap(origin, radius, layerMask);

            foreach (var item in enemyHit)
            {
                if (enemiesAlreadyHit.Contains(item))
                    continue;

                AreaAttacksManager.DealDamageToCollisions<IDamageable>(item, combatData.AttackDamage * spellData.DamageMultiplier);
                enemiesAlreadyHit.Add(item);
            }
        }
    }
}