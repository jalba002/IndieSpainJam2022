using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(ArcSpell), menuName = "CosmosDefender/" + nameof(ArcSpell))]
    public class ArcSpell : BaseSpell
    {
        public VisualEffect vfxPrefab;
        private Coroutine SpellCoroutine;

        public override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation,
            IReadOnlyOffensiveData combatData, ISpellCaster caster)
        {
            caster.Animator.SetTrigger(spellData.AnimationCode);

            if (SpellCoroutine != null)
                CronoScheduler.Instance.StopCoroutine(SpellCoroutine);

            SpellCoroutine = CronoScheduler.Instance.ScheduleForTime(spellData.AnimationDelay, () =>
            {
                var ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
                float cameraCompensatedLength =
                    spellData.MaxAttackDistance +
                    Vector3.Distance(
                        caster.GameObject.transform.position,
                        Camera.main.transform.position);

                bool spawnRay = (Physics.Raycast(ray, out RaycastHit raycastHit, cameraCompensatedLength));

                Vector3 middlePoint = caster.CastingPoint.position + ((spawnPoint - caster.CastingPoint.position) * 0.5f);
                var vfxItem = Instantiate(vfxPrefab, middlePoint, Quaternion.identity);

                vfxItem.SetFloat("Lifetime", spellData.Lifetime);
                Destroy(vfxItem.gameObject, spellData.Lifetime * 1.1f);

                Vector3 endPos = spawnRay ? raycastHit.point : ray.origin + ray.direction * cameraCompensatedLength;
                vfxItem.gameObject.GetComponent<VFXPropertyBinder>().AddPropertyBinder<VFXTransformBinder>().Init("Start", caster.CastingPoint);
                vfxItem.SetVector3("End", endPos);

                var instance = Instantiate(prefab, endPos, rotation);
                instance.InstantiateBullet(endPos, forward, rotation, combatData, currentData, caster);
            });
        }

        public override void StopCast()
        {
            //
        }
    }
}