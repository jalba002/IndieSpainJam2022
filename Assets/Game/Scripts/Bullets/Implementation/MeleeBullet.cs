using System.Collections;
using System.Collections.Generic;
using CosmosDefender.Bullets;
using CosmosDefender.Projectiles;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

namespace CosmosDefender.Bullets.Implementation
{
    public class MeleeBullet : BaseBullet
    {
        [SerializeField] private VisualEffect vfx;

        public override void InstantiateBullet(Vector3 origin, Vector3 forward, Quaternion rotation,
            IReadOnlyOffensiveData combatData, SpellData spellData, ISpellCaster caster)
        {
            base.InstantiateBullet(origin, forward, rotation, combatData, spellData, caster);
            
            SnapToGround();

            if (vfx != null)
            {
                vfx.transform.parent = null;

                vfx.SetFloat("Lifetime", spellData.Lifetime);
                Destroy(vfx.gameObject, spellData.Lifetime);
            }
            
            DealDamage(combatData, caster);

            Destroy(this.gameObject, spellData.Lifetime);
        }

        private void DealDamage(IReadOnlyOffensiveData combatData, ISpellCaster caster)
        {
            var a = AreaAttacksManager.SphereOverlap(
                caster.CastingPoint.position, 
                spellData.ProjectileRadius,
                spellData.LayerMask);
            AreaAttacksManager.DealDamageToCollisions<IDamageable>(a, spellData.DamageMultiplier * combatData.AttackDamage);
        }
    }
}