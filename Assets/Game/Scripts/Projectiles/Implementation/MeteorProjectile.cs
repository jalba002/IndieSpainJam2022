using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

namespace CosmosDefender.Projectiles
{
    public class MeteorProjectile : BaseProjectile
    {
        [SerializeField] private VisualEffect vfx;

        [SerializeField] private ParticleSystem particles;

        [SerializeField] private MeshRenderer mrend;
        
        private SpellData m_SpellData;
        private IReadOnlyOffensiveData m_CombatData;
        private ScreenShake screenShake;

        public override void InitializeProjectile(Vector3 spawnPoint, Vector3 direction, IReadOnlyOffensiveData combatData, SpellData spellData)
        {
            base.InitializeProjectile(spawnPoint, direction, combatData, spellData);
            // Enable collider?
            m_SpellData = spellData;
            m_CombatData = combatData;
            
            m_Rigidbody.velocity = direction * spellData.Speed;
            
            // Update VFXs
            // Set radius
            vfx.SetFloat("ProjectileRadius", m_SpellData.ProjectileRadius);
            vfx.SetFloat("Lifetime", m_SpellData.Lifetime);
            
            // TODO RADIUS BETWEEN VISUALS AND REAL IS 1/5th.
            float realRadius = m_SpellData.ProjectileRadius * 0.2f;
            mrend.material.SetFloat("_Scale", realRadius);

            ((SphereCollider) (m_Collider)).radius = realRadius * 0.5f;

            screenShake = FindObjectOfType<ScreenShake>();

            Destroy(this.gameObject, spellData.ActiveDuration);
        }

        protected override void UpdateVFX()
        {
            vfx.SendEvent("Play");
            vfx.transform.parent = null;
            vfx.transform.position = transform.position; //other.ClosestPoint(transform.position);
            Destroy(vfx.gameObject, m_SpellData.Lifetime * 1.2f);
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
            
            // TODO set particles destroy time.
            CronoScheduler.Instance.ScheduleForTime(3f, () =>
            {
                //particles.gameObject.SetActive(false);
                Destroy(particles.gameObject);
            });
            //particles.gameObject.SetActive(false);
            // Destroy(particles.gameObject, 3f);
        }

        protected override void CastDamage(Vector3 hitPoint)
        {
            // TODO. THE RADIUS FROM SPELLDATA IS 1/5th (THAT OR MULT FOR 5 IN VFX)
            
            var collisions = AreaAttacksManager.SphereOverlap(hitPoint, m_SpellData.ProjectileRadius * 0.3f, m_SpellData.LayerMask);
            
            AreaAttacksManager.DealDamageToCollisions<IDamageable>(collisions, m_CombatData.AttackDamage * m_SpellData.DamageMultiplier);
        }

        protected override void FinishObject()
        {
            Destroy(this.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            // TODO Ignore other projectiles until custom layers are used,
            if (other.GetComponent<BaseProjectile>() != null) return;

            var hitPoint = other.ClosestPointOnBounds(transform.position);
            
            if(impactSound != null)
                impactSound.Play();

            UpdateVFX();
            UpdateRenderer();
            UpdateRigidbody();
            UpdateParticles();
            UpdateCollisions();
            CastDamage(hitPoint);
            screenShake.CameraShake(0.15f, 3f);
            FinishObject();
        }
    }
}