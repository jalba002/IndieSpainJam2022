using System.Collections;
using System.Collections.Generic;
using CosmosDefender.Bullets;
using CosmosDefender.Projectiles;
using UnityEngine;

namespace CosmosDefender.Bullets.Implementation
{
    public class ProjectileBullet : BaseBullet
    {
        [SerializeField] private BaseProjectile prefab;

        //[Button("Test Bullet")]
        public override void InstantiateBullet(Vector3 origin, Vector3 forward, Quaternion rotation,
            IReadOnlyOffensiveData combatData, SpellData spellData, ISpellCaster caster)
        {
            base.InstantiateBullet(origin, forward, rotation, combatData, spellData, caster);

            // Launches a single projectile.
            CronoScheduler.Instance.ScheduleForRepetitions(spellData.Amount, spellData.ProjectileDelay, () =>
            {
                var a = Instantiate(prefab, origin, rotation);
                a.InitializeProjectile(origin, forward, combatData, spellData);
            });
            
            Destroy(this.gameObject, spellData.ProjectileDelay);
        }
    }
}