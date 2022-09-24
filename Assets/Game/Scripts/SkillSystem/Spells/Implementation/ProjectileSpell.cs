using System.Collections;
using System.Collections.Generic;
using CosmosDefender;
using UnityEngine;

namespace CosmosDefender.Projectiles.Implementation
{
    [CreateAssetMenu(fileName = nameof(ProjectileSpell), menuName = "CosmosDefender/" + nameof(ProjectileSpell))]
    public class ProjectileSpell : BaseSpell
    {
        private Coroutine castedSpell;
        public override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation,
            IReadOnlyOffensiveData combatData,
            ISpellCaster caster)
        {
            //throw new System.NotImplementedException();
            
            caster.SetAnimationTrigger(spellData.AnimationCode);
            
            if(castedSpell != null)
                CronoScheduler.Instance.StopCoroutine(castedSpell);

            castedSpell = CronoScheduler.Instance.ScheduleForTime(spellData.AnimationDelay, () =>
            {
                var position = caster.CastingPoint.position;
                var instance = Instantiate(prefab, position, rotation);
                instance.InstantiateBullet(position, forward, rotation, combatData, spellData, caster);
            });
        }

        public override void StopCast()
        {
            //throw new System.NotImplementedException();
        }
    }
}