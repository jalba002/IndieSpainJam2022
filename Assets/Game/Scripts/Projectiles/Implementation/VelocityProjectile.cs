using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

namespace CosmosDefender.Projectiles
{
    public class VelocityProjectile : BaseProjectile
    {
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

            float realRadius = m_SpellData.ProjectileRadius;
            //mrend.material.SetFloat("_Scale", realRadius);

            ((SphereCollider) (m_Collider)).radius = realRadius;
            mrend.gameObject.transform.localScale = new Vector3(realRadius, realRadius, realRadius) * 2f;
            
            Destroy(this.gameObject, spellData.Lifetime);
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
                Destroy(particles.gameObject);
            });
        }

        protected override void FinishObject()
        {
            Destroy(this.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
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