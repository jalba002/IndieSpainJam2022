using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(DashSpell), menuName = "CosmosDefender/" + nameof(DashSpell))]
    public class DashSpell : BaseSpell
    {
        private Coroutine SpellCoroutine;

        protected override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData, SpellManager caster)
        {
            caster.animator.SetTrigger(spellData.AnimationCode);

            if (SpellCoroutine != null)
                CronoScheduler.Instance.StopCoroutine(SpellCoroutine);

            var playerMovement = caster.GetComponent<PlayerMovementController>();
            playerMovement.SetMovementState(false);

            SpellCoroutine = CronoScheduler.Instance.ScheduleForTime(spellData.AnimationDelay, () =>
            {
                playerMovement.Dash(spellData);
            });
        }
    }
}