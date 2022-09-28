using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(LaserSpell), menuName = "CosmosDefender/Spells/" + nameof(LaserSpell))]
    public class LaserSpell : BaseSpell
    {
        private Coroutine SpellCoroutine;

        [SerializeField] private LayerMask _layerMask;
        
        public override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData, ISpellCaster caster)
        {
            caster.Animator.SetTrigger(spellData.AnimationCode);
            
            if(SpellCoroutine != null)
                CronoScheduler.Instance.StopCoroutine(SpellCoroutine);

            SpellCoroutine = CronoScheduler.Instance.ScheduleForTime(spellData.AnimationDelay, () =>
            {
                CronoScheduler.Instance.ScheduleForRepetitions(spellData.Amount, spellData.ProjectileDelay, () =>
                {
                    var ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
                    var position = caster.CastingPoint.position;
                    
                    bool didRayHit = (Physics.Raycast(ray, out RaycastHit info, spellData.MaxAttackDistance, _layerMask));

                    Vector3 a = (didRayHit ? info.point - caster.GameObject.transform.right * 0.2f: (ray.origin + ray.direction * (spellData.MaxAttackDistance * 0.2f))) - position;
                    
                    //a.normalized
                    var bullet = Instantiate(prefab, position, Quaternion.LookRotation(a.normalized));
                    bullet.InstantiateBullet(position, a.normalized, Quaternion.identity, combatData, currentData, caster);
                });
            });
        }
        
        public override void StopCast()
        {
            //
        }
    }
}