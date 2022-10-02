using System;
using System.Linq;
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

        [SerializeField] public VisualEffect feedbackVFX;

        public override void InstantiateBullet(Vector3 origin, Vector3 forward, Quaternion rotation,
            IReadOnlyOffensiveData combatData, SpellData spellData, ISpellCaster caster)
        {
            base.InstantiateBullet(origin, forward, rotation, combatData, spellData, caster);

            // WRONG, USE THE RAYCAST MAX LENGTH
            vfx.Play();
            vfx.SetFloat("Length", spellData.MaxAttackDistance);
            vfx.SetFloat("Width", spellData.ProjectileRadius);
            //vfx.SetVector3("End", endPoint);

            UpdateVFX();

            vfx.SetFloat("Lifetime", spellData.Lifetime);

            // Destroy after maxtime + delay.
        }

        public override void UpdateBullet()
        {
            base.UpdateBullet();
            
            // Cast damage every time it is casted.
            CastRayDamage(
                transform.position,
                transform.forward,
                new Vector3(
                    spellData.ProjectileRadius,
                    spellData.ProjectileRadius,
                    spellData.MaxAttackDistance),
                transform.rotation);
        }

        public override void StopBullet()
        {
            base.StopBullet();
            
            vfx.Stop();
            Destroy(this.gameObject, 0.3f);
        }

        protected override void Update()
        {
            base.Update();

            this.gameObject.transform.position = caster.CastingPoint.position;
            
            // Update this VFX to the raycast Length from this update.
        }

        private void FixedUpdate()
        {
            UpdateVFX();
        }

        private void UpdateVFX()
        {
            Camera cam = Camera.main;
            var ray = cam.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            float raycastDistance =
                (transform.position - cam.transform.position).magnitude + spellData.MaxAttackDistance;

            bool didRayHit = (Physics.Raycast(ray, out RaycastHit info, raycastDistance, raycastHitLayers));

            Vector3 a =
                (
                    didRayHit
                        ? info.point - caster.GameObject.transform.right * 0.2f
                        : (ray.origin + ray.direction.normalized * (raycastDistance))
                )
                - caster.CastingPoint.position;

            // Debug.DrawRay(ray.origin, ray.direction * info.distance, Color.green, 2f);

            transform.forward = a.normalized;
            vfx.SetFloat("Length", a.magnitude);
        }

        private void CastRayDamage(Vector3 origin, Vector3 forward, Vector3 length, Quaternion rotation)
        {
            // TODO Gather information about the ray size here.
            // No multihit? Just one enemy hit.
            
            var hits = AreaAttacksManager.BoxAttack(origin, forward, length * 0.5f, rotation, spellData.LayerMask);
            if (hits.Length <= 0) return;
            
            var enemyIndex = Utils.GetClosestIndexFromList(transform.position, hits.ToList());
            
            // foreach (var enemy in hits)
            // {
            var a = Instantiate(feedbackVFX, hits[enemyIndex].bounds.center, Quaternion.identity);
            Destroy(a, spellData.Lifetime);
            // }

            AreaAttacksManager.DealDamageToCollisions<IDamageable>(hits[enemyIndex], combatData.AttackDamage * spellData.DamageMultiplier);
            //AreaAttacksManager.DealDamageToCollisions<IDamageable>(hits, combatData.AttackDamage * spellData.DamageMultiplier);
        }
    }
}