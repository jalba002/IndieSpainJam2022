using UnityEngine;
using FMODUnity;

namespace CosmosDefender.Projectiles
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class BaseProjectile : MonoBehaviour
    {
        [SerializeField] protected Collider m_Collider;

        [SerializeField] protected Rigidbody m_Rigidbody;

        [SerializeField] protected StudioEventEmitter meteorStart;
        [SerializeField] protected StudioEventEmitter meteorImpact;
        
        public virtual void InitializeProjectile(Vector3 spawnPoint, Vector3 direction, IReadOnlyOffensiveData combatData, SpellData spellData)
        {
            // A+J = Love for ever jj
            meteorStart.Play();
        }

        protected virtual void UpdateRenderer()
        {
        }

        protected virtual void UpdateRigidbody()
        {
        }

        protected virtual void UpdateParticles()
        {
        }

        protected virtual void UpdateCollisions()
        {
        }


        protected virtual void CastDamage(Vector3 pos)
        {
        }

        protected virtual void FinishObject()
        {
        }

        protected virtual void UpdateVFX()
        {
        }
    }
}