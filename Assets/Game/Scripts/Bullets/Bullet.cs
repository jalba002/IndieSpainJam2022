using UnityEngine;

namespace CosmosDefender.Bullets
{
// The bullet work is to instantiate the projectile using a valid position.
    public class BaseBullet : MonoBehaviour
    {
        [Header("Misc")] [SerializeField] protected bool m_SnapToGround = false;

        [SerializeField] protected bool m_CheckOutOfBounds = false;

        [Header("Snap to ground Raycast settings")] [SerializeField]
        protected LayerMask raycastHitLayers;

        [SerializeField] protected float rayToGroundDistance = 10f;

        protected bool IsStopped = false;

        // Components
        protected Rigidbody _rb;

        public virtual void InstantiateBullet()
        {
            
        }

        public virtual void InstantiateBullet(Vector3 origin, Vector3 forward, Quaternion rotation,
            IReadOnlyOffensiveData combatData, SpellData spellData)
        {
            // Maybe its a good choice to give it some instructions.
            _rb = GetComponent<Rigidbody>();
            // no problem if there's none.
        }

        protected virtual void Update()
        {
            // Snap to ground and check if its out of ground too.
            if (m_CheckOutOfBounds)
                CheckOutOfGround();
            if (m_SnapToGround)
                SnapToGround();
        }

        protected virtual void CheckOutOfGround()
        {
            if (IsStopped) return;

            RaycastHit hit;

            Vector3 distance = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
            if (Physics.Raycast(distance, transform.TransformDirection(-Vector3.up), out hit, rayToGroundDistance,
                raycastHitLayers))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
            else
            {
                //transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
                IsStopped = true;
                _rb.velocity = Vector3.zero;
            }
        }

        protected virtual void SnapToGround()
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out RaycastHit hit,
                rayToGroundDistance, raycastHitLayers))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
        }
    }
}