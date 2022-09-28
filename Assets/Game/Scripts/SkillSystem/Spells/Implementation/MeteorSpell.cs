using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(MeteorSpell), menuName = "CosmosDefender/Spells/" + nameof(MeteorSpell))]
    public class MeteorSpell : BaseSpell
    {
        private Coroutine SpellCoroutine;
        public override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData, ISpellCaster caster)
        {
            caster.SetAnimationTrigger(spellData.AnimationCode);
            
            if(SpellCoroutine != null)
                CronoScheduler.Instance.StopCoroutine(SpellCoroutine);

            SpellCoroutine = CronoScheduler.Instance.ScheduleForTime(spellData.AnimationDelay, () =>
            {
                var instance = Instantiate(prefab, spawnPoint, rotation);
                instance.InstantiateBullet(spawnPoint, forward, rotation, combatData, currentData, caster);
            });
        }
        
        public override void StopCast()
        {
            //
        }
    }
}