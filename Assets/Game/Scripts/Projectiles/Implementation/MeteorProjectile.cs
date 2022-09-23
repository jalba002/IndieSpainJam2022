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
        
        [SerializeField] private LayerMask layerMask;

        private SpellData m_SpellData;
        private IReadOnlyOffensiveData m_CombatData;

        public override void InitializeProjectile(Vector3 spawnPoint, IReadOnlyOffensiveData combatData, SpellData spellData)
        {
            base.InitializeProjectile(spawnPoint, combatData, spellData);
            // Enable collider?
            m_SpellData = spellData;
            m_CombatData = combatData;
            
            m_Rigidbody.velocity = Vector3.down * spellData.Speed;
            
            // Update VFXs
            // Set radius
            vfx.SetFloat("ProjectileRadius", m_SpellData.ProjectileRadius);
            vfx.SetFloat("Lifetime", m_SpellData.Lifetime);
            
            // TODO RADIUS BETWEEN VISUALS AND REAL IS 1/5th.
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

        protected override void CastDamage(Vector3 hitPoint)
        {
            // TODO. THE RADIUS FROM SPELLDATA IS 1/5th (THAT OR MULT FOR 5 IN VFX)
            
            var collisions = AreaAttacksManager.SphereOverlap(hitPoint, m_SpellData.ProjectileRadius * 0.3f, layerMask);
            
            AreaAttacksManager.DealDamageToCollisions<IDamageable>(collisions, m_CombatData.AttackDamage * m_SpellData.DamageMultiplier);
        }

        protected override void FinishObject()
        {
            // TODO switch this if there's a pooler somehow.
            Destroy(this.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            // TODO Ignore other projectiles until custom layers are used,
            if (other.GetComponent<BaseProjectile>() != null) return;

            var hitPoint = other.ClosestPointOnBounds(transform.position);
            
            UpdateVFX();
            UpdateRenderer();
            UpdateRigidbody();
            UpdateParticles();
            UpdateCollisions();
            CastDamage(hitPoint);
            FinishObject();
        }
    }
}