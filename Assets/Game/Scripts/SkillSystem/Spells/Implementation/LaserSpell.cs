using CosmosDefender.Bullets.Implementation;
using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(LaserSpell), menuName = "CosmosDefender/Spells/" + nameof(LaserSpell))]
    public class LaserSpell : BaseSpell
    {
        private LaserBullet instantiatedBullet;
        private ISpellCaster caster;
        private float nextCast = 0f;
        private bool firstCast = false;
        private float firstCastTime;
        
        public override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData, ISpellCaster caster)
        {
            if (firstCast)
            {
                // Set a delay for the first instantiation.
                firstCastTime = Time.time + spellData.AnimationDelay;
                firstCast = false;
                Debug.Log("Casting laser for first time.");
                caster.Animator.SetBool(spellData.AnimationCode, true);
            }

            if (Time.time < firstCastTime) return;
            
            if (instantiatedBullet == null)
            {
                Debug.Log("Instantiating Laser Bullet");
                this.caster = caster;
                
                var ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
                var position = caster.CastingPoint.position;
                    
                bool didRayHit = (Physics.Raycast(ray, out RaycastHit info, spellData.MaxAttackDistance, spellData.LayerMask));

                Vector3 a = (didRayHit ? info.point - caster.GameObject.transform.right * 0.2f: (ray.origin + ray.direction * (spellData.MaxAttackDistance * 0.2f))) - position;
                
                instantiatedBullet = Instantiate(prefab, position, Quaternion.LookRotation(a.normalized)) as LaserBullet;
                instantiatedBullet.InstantiateBullet(position, a.normalized, Quaternion.identity, combatData, currentData, caster);
                instantiatedBullet.UpdateBullet();
                nextCast = Time.time + spellData.ProjectileDelay;
            }
            else
            {
                // Set a delay to deal damage.
                if (Time.time >= nextCast)
                {
                    instantiatedBullet.UpdateBullet();
                    nextCast = Time.time + spellData.ProjectileDelay;
                }
            }
        }
        
        public override void StopCast()
        {
            if(instantiatedBullet != null)
                instantiatedBullet.StopBullet();
            
            instantiatedBullet = null;
            if(caster.Animator != null)
                caster.Animator.SetBool(spellData.AnimationCode, false);
            firstCast = true;
        }
    }
}
