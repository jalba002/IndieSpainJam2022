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

                Transform firePoint = FindObjectOfType<SpellManager>().FirePoint;
                var spellTesterFirePointPosition = firePoint.position;
                Vector3 sp = raycastHit.point - (raycastHit.point - spellTesterFirePointPosition) / 2;
                var vfxItem = Instantiate(vfxPrefab, sp, Quaternion.identity);
                vfxItem.gameObject.GetComponent<VFXPropertyBinder>().AddPropertyBinder<VFXTransformBinder>()
                    .Init("Start", firePoint);
                //vfxItem.SetVector3("Start", spellTesterFirePointPosition);

                vfxItem.SetFloat("Lifetime", spellData.Lifetime);
                Destroy(vfxItem.gameObject, spellData.Lifetime * 1.1f);

                Vector3 spawnPos = spawnRay ? raycastHit.point : ray.origin + ray.direction * cameraCompensatedLength;
                vfxItem.SetVector3("End", spawnPos);

                var instance = Instantiate(prefab, spawnPos, rotation);
                instance.InstantiateBullet(spawnPos, forward, rotation, combatData, currentData, caster);
            });
        }

        public override void StopCast()
        {
            //
        }
    }
}