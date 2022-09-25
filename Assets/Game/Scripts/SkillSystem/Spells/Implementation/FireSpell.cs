using CosmosDefender.Bullets;
using CosmosDefender.Bullets.Implementation;
using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(FireSpell), menuName = "CosmosDefender/" + nameof(FireSpell))]
    public class FireSpell : BaseSpell
    {
        private Coroutine SpellCoroutine;

        private FireBullet instance;

        public override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation,
            IReadOnlyOffensiveData combatData, ISpellCaster caster)
        {
            caster.Animator.SetTrigger(spellData.AnimationCode);

            if (SpellCoroutine != null)
                CronoScheduler.Instance.StopCoroutine(SpellCoroutine);

            if (instance == null)
            {
                instance = Instantiate(prefab, spawnPoint, rotation) as FireBullet;
                instance.InstantiateBullet(spawnPoint, forward, rotation, combatData, spellData, caster);
            }

            SpellCoroutine = CronoScheduler.Instance.ScheduleForTime(spellData.AnimationDelay,
                () =>
                {
                    instance.Activate();
                });
        }

        public override void StopCast()
        {
            instance.Deactivate();
        }
    }
}