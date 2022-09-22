using UnityEngine;
using UnityEngine.VFX;

namespace CosmosDefender.Bullets.Implementation
{
    public class LaserBullet : BaseBullet
    {
        // This laser starts in a position
        // Then casts a large box periodically, every 0.25 secs.
        // That uses the External Coroutine manager with an action.
        // CronoScheduler.Instance.ScheduleForTime();
        [SerializeField] private VisualEffect vfx;

        protected BulletInfo m_BulletInfo;

        public override void InstantiateBullet(Vector3 origin, Vector3 forward, Quaternion rotation,
            IReadOnlyOffensiveData combatData, SpellData spellData)
        {
            base.InstantiateBullet(origin, forward, rotation, combatData, spellData);

            Vector3 maxLength = new Vector3(1f, 1f, 20f);
            m_BulletInfo = new BulletInfo(transform, maxLength);
            // NO NEED, THIS IS THE HEIGHT.
            // vfx.gameObject.transform.localScale = new Vector3(maxLength.x, maxLength.z, maxLength.y);
            // IN THE END, MODIFY THE LENGTH Z with RAYCAST. So it collides with pillars.

            this.transform.forward = forward;
            this.transform.rotation = rotation;
            // Need whoever instantiated it.
            //this.transform.parent = origin;

            // TODO Gather length size and send here.
            CronoScheduler.Instance.ScheduleForTimeAndExecuteElapsed(3f,
                0.25f,
                () => CastRayDamage(ref m_BulletInfo));

            // Destroy after maxtime + delay.
            Destroy(this.gameObject, 3.5f);
        }

        protected override void Update()
        {
            base.Update();
            m_BulletInfo.UpdateInfo(transform);
        }

        protected struct BulletInfo
        {
            public Transform currentTransform;
            public Vector3 maxLength;

            public BulletInfo(Transform t, Vector3 mL)
            {
                currentTransform = t;
                maxLength = mL;
            }

            public void UpdateInfo(Transform t)
            {
                currentTransform = t;
            }
        }

        private void CastRayDamage(ref BulletInfo bInfo)
        {
            // TODO Gather information about the ray size here.
            var hits = AreaAttacksManager.BoxAttack(
                bInfo.currentTransform.position,
                bInfo.currentTransform.forward, bInfo.maxLength * 0.5f,
                bInfo.currentTransform.rotation, 0.1f);

            // Treat all hits.
            string hitLog = "Hit Log:\n";
            foreach (var collisionHit in hits)
            {
                // Treat collision hits.
                hitLog += $"{collisionHit.gameObject.name}\n";
            }

            Debug.Log(hitLog);
        }

        //
        // private void CastRayDamage(Vector3 origin, Vector3 length, Quaternion rotation)
        // {
        //     // TODO Gather information about the ray size here.
        //     AreaAttacksManager.BoxAttack(this.transform.position, length * 0.5f, rotation, 0.1f);
        // }
    }
}