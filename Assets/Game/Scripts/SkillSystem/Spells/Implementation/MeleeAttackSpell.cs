using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(MeleeAttackSpell), menuName = "CosmosDefender/" + nameof(MeleeAttackSpell))]
    public class MeleeAttackSpell : BaseSpell
    {
        private Coroutine delayedDamage;
        private Coroutine delayedAnimation;
        
        public override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation,
            IReadOnlyOffensiveData combatData,
            ISpellCaster caster)
        {
            caster.SetAnimationTrigger(spellData.AnimationCode);

            //Collider handCollision = caster.CastingPoint.GetComponent<Collider>();
            //((SphereCollider) handCollision).radius = spellData.ProjectileRadius;
            if(delayedAnimation != null)
                CronoScheduler.Instance.StopCoroutine(delayedAnimation);
            
            delayedAnimation = CronoScheduler.Instance.ScheduleForTime(spellData.AnimationDelay, () =>
            {
                // Activate the collider for the animation
                //handCollision.enabled = true;
                if(delayedDamage != null)
                    CronoScheduler.Instance.StopCoroutine(delayedDamage);
                
                delayedDamage = CronoScheduler.Instance.ScheduleForRepetitions(spellData.Amount, spellData.ProjectileDelay, () =>
                {
                    //handCollision.enabled = false;
                    // Cast the sphere here and deal damage instant
                    DealDamage(combatData, caster);
                });
            });
        }

        private void DealDamage(IReadOnlyOffensiveData combatData, ISpellCaster caster)
        {
            var a = AreaAttacksManager.SphereOverlap(
                caster.CastingPoint.position, 
                spellData.ProjectileRadius,
                spellData.LayerMask);
            AreaAttacksManager.DealDamageToCollisions<IDamageable>(a, spellData.DamageMultiplier * combatData.AttackDamage);
        }

        public override void StopCast()
        {
        }
    }
}