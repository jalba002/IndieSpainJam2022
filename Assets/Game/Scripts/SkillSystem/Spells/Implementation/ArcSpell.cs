using System.Dynamic;
using Unity.VisualScripting;
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

        protected override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation,
            IReadOnlyOffensiveData combatData, SpellManager caster)
        {
            caster.animator.SetTrigger(spellData.AnimationCode);
            
            if(SpellCoroutine != null)
                CronoScheduler.Instance.StopCoroutine(SpellCoroutine);

            SpellCoroutine = CronoScheduler.Instance.ScheduleForTime(spellData.AnimationDelay, () =>
            {
                var ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
                bool spawnRay = (Physics.Raycast(ray, out RaycastHit raycastHit, spellData.MaxAttackDistance));

                Transform firePoint = FindObjectOfType<SpellManager>().FirePoint;
                var spellTesterFirePointPosition = firePoint.position;
                Vector3 sp = raycastHit.point - (raycastHit.point - spellTesterFirePointPosition) / 2;
                var vfxItem = Instantiate(vfxPrefab, sp, Quaternion.identity);
                vfxItem.gameObject.GetComponent<VFXPropertyBinder>().AddPropertyBinder<VFXTransformBinder>().Init("Start", firePoint);
                //vfxItem.SetVector3("Start", spellTesterFirePointPosition);
                vfxItem.SetVector3("End", raycastHit.point);

                vfxItem.SetFloat("Lifetime", spellData.Lifetime);
                Destroy(vfxItem.gameObject, spellData.Lifetime * 1.1f);

                if (spawnRay)
                {
                    var instance = Instantiate(prefab, raycastHit.point, rotation);
                    instance.InstantiateBullet(raycastHit.point, forward, rotation, combatData, currentData);
                }
            });
        }
    }
}