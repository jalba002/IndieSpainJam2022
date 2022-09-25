using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(DashSpell), menuName = "CosmosDefender/" + nameof(DashSpell))]
    public class DashSpell : BaseSpell
    {
        private Coroutine SpellCoroutine;

        public override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData, ISpellCaster caster)
        {
            caster.Animator.SetTrigger(spellData.AnimationCode);
            caster.GameObject.GetComponent<PlayerHealthManager>().InvulnerableOverTime(spellData.ActiveDuration + 1f);

            if (SpellCoroutine != null)
                CronoScheduler.Instance.StopCoroutine(SpellCoroutine);

            var playerMovement = caster.GameObject.GetComponent<PlayerMovementController>();
            playerMovement.SetMovementState(false);

            SpellCoroutine = CronoScheduler.Instance.ScheduleForTime(spellData.AnimationDelay, () =>
            {
                playerMovement.Dash(spellData);
                var instance = Instantiate(prefab, spawnPoint, rotation);
                instance.InstantiateBullet(spawnPoint, forward, rotation, combatData, spellData, caster);
                //instance.transform.SetParent(playerMovement.PlayerCenter);
            });
        }

        public override void StopCast()
        {
            //
        }
    }
}