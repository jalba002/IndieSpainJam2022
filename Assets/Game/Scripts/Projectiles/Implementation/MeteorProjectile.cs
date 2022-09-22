using UnityEngine;
using UnityEngine.VFX;

namespace CosmosDefender.Projectiles
{
    public class MeteorProjectile : BaseProjectile
    {
        [SerializeField] private VisualEffect vfx;

        [SerializeField] private ParticleSystem particles;

        [SerializeField] private MeshRenderer mrend;

        public override void InitializeProjectile(Vector3 spawnPoint, Vector3 velocity)
        {
            base.InitializeProjectile(spawnPoint, velocity);
            // Enable collider?
            m_Rigidbody.velocity = velocity;
        }

        protected override void UpdateVFX()
        {
            vfx.SendEvent("Play");
            vfx.transform.parent = null;
            vfx.transform.position = transform.position; //other.ClosestPoint(transform.position);
            Destroy(vfx.gameObject, 10f);
        }

        protected override void UpdateRenderer()
        {
            mrend.enabled = false;
        }

        protected override void UpdateRigidbody()
        {
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.useGravity = false;
        }

        protected override void UpdateCollisions()
        {
            m_Collider.enabled = false;
        }

        protected override void UpdateParticles()
        {
            particles.Stop();
            particles.gameObject.transform.parent = null;
            
            // TODO set in time.
            CronoScheduler.Instance.ScheduleForTime(3f, () =>
            {
                //particles.gameObject.SetActive(false);
                Destroy(particles.gameObject);
            });
            //particles.gameObject.SetActive(false);
            // Destroy(particles.gameObject, 3f);
        }

        protected override void CastDamage()
        {
            var a = AreaAttacksManager.SphereOverlap(this.gameObject.transform.position, 3f, true);
            
            // TODO get the correct damage from the spells settings.
            AreaAttacksManager.DealDamageToCollisions<IDamageable>(a, 10f);
        }

        protected override void FinishObject()
        {
            // TODO switch this if there's a pooler somehow.
            Destroy(this.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            UpdateVFX();
            UpdateRenderer();
            UpdateRigidbody();
            UpdateParticles();
            UpdateCollisions();
            CastDamage();
            FinishObject();
        }
    }
}