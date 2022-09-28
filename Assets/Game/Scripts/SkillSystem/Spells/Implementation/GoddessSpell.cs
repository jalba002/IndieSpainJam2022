using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(GoddessSpell),
        menuName = "CosmosDefender/Spells/Ultimates/" + nameof(GoddessSpell))]
    public class GoddessSpell : BaseSpell
    {
        [Header("SpellData")] [Space(5)] [SerializeField]
        private VisualEffect buffVFXPrefab;

        private Coroutine SpellCoroutine;

        public override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation,
            IReadOnlyOffensiveData combatData, ISpellCaster caster)
        {
            caster.Animator.SetTrigger(spellData.AnimationCode);

            caster.GameObject.GetComponent<MaterialModifier>().ChangeMaterial(true);
            
            GameManager.Instance.ActivateGoddessMode();

            if (SpellCoroutine != null)
                CronoScheduler.Instance.StopCoroutine(SpellCoroutine);
            
            SpellCoroutine = CronoScheduler.Instance.ScheduleForTime(spellData.AnimationDelay,
                () =>
                {
                    CastSpell(spawnPoint, forward, rotation, combatData, caster);

                    CronoScheduler.Instance.ScheduleForTime(spellData.ActiveDuration,
                        () => { caster.GameObject.GetComponent<MaterialModifier>().ChangeMaterial(false); });
                });
        }

        private void CastSpell(Vector3 spawnPoint, Vector3 forward, Quaternion rotation,
            IReadOnlyOffensiveData combatData, ISpellCaster caster)
        {
            // Activate buff delayed.
            // Setup Material.
            // And other stuff.
        }

        public override void StopCast()
        {
            //
        }
    }
}