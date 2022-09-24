using System;
using System.Collections;
using System.Collections.Generic;
using CosmosDefender.Bullets;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CosmosDefender.Bullets.Implementation
{
    public class FireBullet : BaseBullet
    {
        [SerializeField] private ParticleSystem particleSys;
        
        public override void InstantiateBullet(Vector3 origin, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData,
            SpellData spellData, ISpellCaster caster)
        {
            base.InstantiateBullet(origin, forward, rotation, combatData, spellData, caster);
            
            // This object does NOT destroy itself.
            // Every time it is activated, call this function.
            
            Activate();
        }

        [Button]
        void Activate()
        {
            particleSys.Play();
        }

        [Button]
        public void Deactivate()
        {
            particleSys.Stop();
        }
        
        void TreatParticleCollision(Collider go)
        {
            AreaAttacksManager.DealDamageToCollisions<IDamageable>(go, combatData.AttackDamage * spellData.DamageMultiplier);
        }
    }
}