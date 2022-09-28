using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(MeleeAttackSpell), menuName = "CosmosDefender/Spells/" + nameof(MeleeAttackSpell))]
    public class MeleeAttackSpell : BaseSpell
    {
        private Coroutine delayedDamage;
        private Coroutine delayedAnimation;
        
        public override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData, ISpellCaster caster)
        {
            caster.SetAnimationTrigger(spellData.AnimationCode);

            if(delayedAnimation != null)
                CronoScheduler.Instance.StopCoroutine(delayedAnimation);
            
            delayedAnimation = CronoScheduler.Instance.ScheduleForTime(spellData.AnimationDelay, () =>
            {
                if(delayedDamage != null)
                    CronoScheduler.Instance.StopCoroutine(delayedDamage);
                
                delayedDamage = CronoScheduler.Instance.ScheduleForTime(spellData.ProjectileDelay, () =>
                {
                    var a = Instantiate(prefab, spawnPoint, Quaternion.identity);
                    a.InstantiateBullet(spawnPoint, forward, Quaternion.identity, combatData ,spellData, caster);
                });
            });
        }
        
        public override void StopCast()
        {
            if (delayedDamage != null)
                CronoScheduler.Instance.StopCoroutine(delayedDamage);
            if (delayedAnimation != null)
                CronoScheduler.Instance.StopCoroutine(delayedAnimation);
        }
    }
}