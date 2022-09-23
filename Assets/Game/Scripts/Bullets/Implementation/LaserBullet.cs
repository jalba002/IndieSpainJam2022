using System;
using UnityEngine;
using UnityEngine.VFX;

namespace CosmosDefender.Bullets.Implementation
{
    public class LaserBullet : BaseBullet
    {
        // This laser starts in a position
        // Then casts a large box periodically, every N secs.
        // That uses the External Coroutine manager with an action.
        // CronoScheduler.Instance.ScheduleForTime();

        [SerializeField] public VisualEffect vfx;

        private IReadOnlyOffensiveData combatData;

        private SpellData spellData;

        private Vector3 origin;

        private Transform firePoint;

        private SpellManager caster;

        public override void InstantiateBullet(Vector3 origin, Vector3 forward, Quaternion rotation,
            IReadOnlyOffensiveData combatData, SpellData spellData)
        {
            base.InstantiateBullet(origin, forward, rotation, combatData, spellData);

            this.spellData = spellData;
            this.combatData = combatData;

            this.origin = origin;

            firePoint = FindObjectOfType<SpellManager>().FirePoint;
            caster = firePoint.GetComponentInParent<SpellManager>();


            //this.transform.forward = forward;
            //this.transform.rotation = rotation;
            //this.transform.parent = origin;

            // Setup VFX here.
            //vfxItem.gameObject.GetComponent<VFXPropertyBinder>().AddPropertyBinder<VFXTransformBinder>().Init("Start", firePoint);
            //vfxItem.SetVector3("Start", spellTesterFirePointPosition);

            // vfx.SetVector3("Direction", forward);

            // WRONG, USE THE RAYCAST MAX LENGTH
            //vfx.SetFloat("Length", spellData.MaxAttackDistance * 0.2f);
            //vfx.SetFloat("Width", spellData.ProjectileRadius);
            //vfx.SetVector3("End", endPoint);

            UpdateVFX();

            vfx.SetFloat("Lifetime", spellData.Lifetime);

            CronoScheduler.Instance.ScheduleForTime(spellData.ProjectileDelay, () => CastRayDamage(
                transform.position,
                transform.forward,
                new Vector3(
                    spellData.ProjectileRadius,
                    spellData.ProjectileRadius,
                    spellData.MaxAttackDistance),
                transform.rotation));

            // Destroy after maxtime + delay.
            Destroy(this.gameObject, spellData.Lifetime * 1.5f);
        }

        protected override void Update()
        {
            base.Update();

            this.gameObject.transform.position = firePoint.position;
        }

        private void FixedUpdate()
        {
            UpdateVFX();
        }

        private void UpdateVFX()
        {
            Camera cam = Camera.main;
            var ray = cam.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            float raycastDistance = (transform.position - cam.transform.position).magnitude + spellData.MaxAttackDistance;

            bool didRayHit = (Physics.Raycast(ray, out RaycastHit info, raycastDistance, raycastHitLayers));

            Vector3 a = 
            (
                didRayHit ? 
                    info.point - caster.transform.right * 0.2f
                    : 
                    (ray.origin + ray.direction.normalized * (raycastDistance))
                )
            - firePoint.position;
            
            // Debug.DrawRay(ray.origin, ray.direction * info.distance, Color.green, 2f);
            
            transform.forward = a.normalized;
            //vfx.SetFloat("Length", didRayHit ? info.distance * 0.2f : spellData.MaxAttackDistance * 0.2f);

            // vfx.SetBool("Rayhit", didRayHit);
            //
            // if (didRayHit)
            // {
            //     vfx.SetFloat("RayDistance", info.distance * 0.2f);
            //     //Vector3 endPoint = info.point - caster.gameObject.transform.right * 0.2f;
            //
            //     //transform.forward = endPoint.normalized;
            // }
        }

        private void CastRayDamage(Vector3 origin, Vector3 forward, Vector3 length, Quaternion rotation)
        {
            // TODO Gather information about the ray size here.
            AreaAttacksManager.BoxAttack(origin, forward, length * 0.5f, rotation, 0.1f);
        }
    }
}