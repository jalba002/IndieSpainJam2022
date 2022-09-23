using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(MeteorSpell), menuName = "CosmosDefender/" + nameof(MeteorSpell))]
    public class MeteorSpell : BaseSpell
    {
        private Coroutine SpellCoroutine;
        protected override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData, SpellManager caster)
        {
            caster.animator.SetTrigger(spellData.AnimationCode);
            
            if(SpellCoroutine != null)
                CronoScheduler.Instance.StopCoroutine(SpellCoroutine);

            SpellCoroutine = CronoScheduler.Instance.ScheduleForTime(spellData.AnimationDelay, () =>
            {
                var instance = Instantiate(prefab, spawnPoint, rotation);
                instance.InstantiateBullet(spawnPoint, forward, rotation, combatData, currentData);
            });
        }
    }
}