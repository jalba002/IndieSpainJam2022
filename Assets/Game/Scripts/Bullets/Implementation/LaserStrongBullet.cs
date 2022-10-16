using UnityEngine;
using UnityEngine.VFX;

namespace CosmosDefender.Bullets.Implementation
{
    public class LaserStrongBullet : BaseBullet
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

            vfx.Play();
            vfx.SetFloat("Length", spellData.MaxAttackDistance);
            vfx.SetFloat("Width", spellData.ProjectileRadius);

            UpdateVFX();

            vfx.SetFloat("Lifetime", spellData.Lifetime);
        }

        public override void UpdateBullet()
        {
            base.UpdateBullet();

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
            this.gameObject.transform.position = caster.CastingPoint.position;
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

            transform.forward = a.normalized;
            vfx.SetFloat("Length", a.magnitude);
        }

        private void CastRayDamage(Vector3 origin, Vector3 forward, Vector3 length, Quaternion rotation)
        {
            var hits = AreaAttacksManager.BoxAttack(origin, forward, length * 0.5f, rotation, spellData.LayerMask);
            if (hits.Length <= 0) return;

            foreach (var enemy in hits)
            {
                var a = Instantiate(feedbackVFX, enemy.bounds.center, Quaternion.identity);
                Destroy(a, spellData.Lifetime);
            }

            AreaAttacksManager.DealDamageToCollisions<IDamageable>(hits,
                combatData.AttackDamage * spellData.DamageMultiplier);
        }
    }
}