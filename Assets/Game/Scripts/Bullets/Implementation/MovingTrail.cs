using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace CosmosDefender.Bullets.Implementation
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovingTrail : BaseBullet
    {
        public float bulletSpeed = 1f;

        public override void InstantiateBullet(Vector3 origin, Vector3 forward, Quaternion rotation,
            IReadOnlyOffensiveData combatData, SpellData spellData, ISpellCaster caster)
        {
            base.InstantiateBullet(origin, forward, rotation, combatData, spellData, caster);
            // Setup and coroutine.
            // Get VFX duration.
            // Get component damage.
            // Calculate total.

            VisualEffect vfx = GetComponentInChildren<VisualEffect>();
            _rb = GetComponent<Rigidbody>();
            // Get from prefab or VFX.
            float trailDuration = vfx.GetFloat("Duration");
            float groundDuration = vfx.GetFloat("MaxDecalDuration");

            _rb.velocity = transform.forward * bulletSpeed;

            StartCoroutine(DelayedMovingTrail(trailDuration, groundDuration));
        }

        private void Update()
        {
            CheckOutOfGround();
            SnapToGround();
        }

        private void OnTriggerEnter(Collider other)
        {
            //
            Debug.Log($"{this.name} collided with {other.gameObject.name}");
        }

        IEnumerator DelayedMovingTrail(float delay, float destroyDelay, bool destroy = true)
        {
            //AreaAttacksManager.SphereOverlap(transform.position, 5f);
            yield return new WaitForSeconds(delay);
            _rb.velocity = Vector3.zero;
            //this.gameObject.SetActive(false);
            yield return new WaitForSeconds(destroyDelay + .5f);
            if (destroy)
            {
                Destroy(this.gameObject);
            }
            else
            {
                this.gameObject.SetActive(false);
            }

            // Deactivate if object pooler.
        }
    }
}