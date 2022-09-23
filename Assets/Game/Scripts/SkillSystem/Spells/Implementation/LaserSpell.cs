using CosmosDefender.Bullets;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(LaserSpell), menuName = "CosmosDefender/" + nameof(LaserSpell))]
    public class LaserSpell : BaseSpell
    {
        private Coroutine SpellCoroutine;

        [SerializeField] private LayerMask _layerMask;
        
        protected override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData, SpellManager caster)
        {
            // Custom cast
            // Laser is cast from the hand, laser casts multiple times on empowered.
            
            caster.animator.SetTrigger(spellData.AnimationCode);
            
            if(SpellCoroutine != null)
                CronoScheduler.Instance.StopCoroutine(SpellCoroutine);

            SpellCoroutine = CronoScheduler.Instance.ScheduleForTime(spellData.AnimationDelay, () =>
            {
                Transform firePoint = FindObjectOfType<SpellManager>().FirePoint;
                //var spellTesterFirePointPosition = firePoint.position;
                //Vector3 sp = raycastHit.point - (raycastHit.point - spellTesterFirePointPosition) / 2;

                CronoScheduler.Instance.ScheduleForRepetitions(spellData.Amount, spellData.ProjectileDelay, () =>
                {
                    var ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
                    bool didRayHit = (Physics.Raycast(ray, out RaycastHit info, spellData.MaxAttackDistance, _layerMask));

                    Vector3 a = (didRayHit ? info.point - caster.transform.right * 0.2f: (ray.origin + ray.direction * (spellData.MaxAttackDistance * 0.2f))) - firePoint.position;
                    
                    //a.normalized
                    var bullet = Instantiate(prefab, firePoint.position, Quaternion.LookRotation(a.normalized));
                    bullet.InstantiateBullet(firePoint.position, a.normalized, Quaternion.identity, combatData, currentData);
                    return;
                    
                    // var ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
                    // bool didRayHit = (Physics.Raycast(ray, out RaycastHit raycastHit, spellData.MaxAttackDistance, _layerMask));
                    // Vector3 endPoint;
                    // Vector3 endDirection;
                    // float length;
                    // if (didRayHit)
                    // {
                    //     endPoint = raycastHit.point - caster.gameObject.transform.right * 0.2f;
                    // }
                    // else
                    // {
                    //     endPoint = ray.origin + ray.direction * spellData.MaxAttackDistance;
                    //     // This is length too.
                    // }
                    // endDirection = endPoint - firePoint.position;
                    // length = didRayHit ? endDirection.magnitude : spellData.MaxAttackDistance;
                    //
                    //
                    // var bullet = Instantiate(prefab, firePoint.position, Quaternion.LookRotation(endDirection));
                    // bullet.InstantiateBullet(firePoint.position, endDirection.normalized, Quaternion.LookRotation(endDirection), combatData, currentData);
                   
                });
            });
            
            // var instance = Instantiate(prefab, spawnPoint, rotation);
            // instance.InstantiateBullet(spawnPoint, forward, rotation, combatData, currentData);
        }
    }
}