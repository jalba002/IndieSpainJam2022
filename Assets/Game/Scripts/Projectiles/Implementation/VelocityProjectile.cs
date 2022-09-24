using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

namespace CosmosDefender.Projectiles
{
    public class VelocityProjectile : BaseProjectile
    {
        [SerializeField] private VisualEffect vfx;

        [SerializeField] private ParticleSystem particles;

        [SerializeField] private MeshRenderer mrend;

        private SpellData m_SpellData;
        private IReadOnlyOffensiveData m_CombatData;

        public override void InitializeProjectile(Vector3 spawnPoint, Vector3 direction,
            IReadOnlyOffensiveData combatData, SpellData spellData)
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

            float realRadius = m_SpellData.ProjectileRadius * 0.2f;
            mrend.material.SetFloat("_Scale", realRadius);

            ((SphereCollider) (m_Collider)).radius = realRadius * 0.5f;
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

        protected override void FinishObject()
        {
            Destroy(this.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            // TODO Ignore other projectiles until custom layers are used,
            if (other.GetComponent<BaseProjectile>() != null) return;

            AreaAttacksManager.DealDamageToCollisions<IDamageable>(other,
                m_CombatData.AttackDamage * m_SpellData.DamageMultiplier);

            UpdateVFX();
            UpdateRenderer();
            UpdateRigidbody();
            UpdateParticles();
            UpdateCollisions();
            FinishObject();
        }
    }
}