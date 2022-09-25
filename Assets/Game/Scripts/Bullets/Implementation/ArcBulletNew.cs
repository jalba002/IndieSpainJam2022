using System.Collections.Generic;
using System.Linq;
using CosmosDefender.Projectiles;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

namespace CosmosDefender.Bullets.Implementation
{
    public class ArcBulletNew : BaseBullet
    {
        [SerializeField] private VisualEffect vfxPrefab;

        private List<Collider> enemyHits = new List<Collider>();

        public override void InstantiateBullet(Vector3 origin, Vector3 forward, Quaternion rotation,
            IReadOnlyOffensiveData combatData, SpellData spellData, ISpellCaster caster)
        {
            base.InstantiateBullet(origin, forward, rotation, combatData, spellData, caster);

            var a = AreaAttacksManager.SphereOverlap(origin, spellData.UniformSize, spellData.LayerMask);
            
            int index = GetClosestIndexFromList(a.ToList());
            
            if (a.Length > 0 && a[index].GetComponent<IDamageable>() != null)
            {
                enemyHits.Add(a[index]);
                DetectAllEnemies(enemyHits[index].transform.position, combatData, spellData);

                Bind(origin, enemyHits[index].bounds.center);
            }

            CronoScheduler.Instance.ScheduleForTime(
                spellData.Amount * spellData.ProjectileDelay + 1f,
                () => { Destroy(this.gameObject); });
        }

        private void Bind(Vector3 posOne, Vector3 posTwo)
        {
            Vector3 spawnP = (posOne + posTwo) * 0.5f;

            var vfxItem = Instantiate(vfxPrefab, spawnP, Quaternion.identity);
            //vfxItem.SetVector3("Start", posOne);
            //vfxItem.SetVector3("End", posTwo);
            //vfxItem.SetFloat("Lifetime", spellData.Lifetime);
            vfxItem.GetComponent<ArcSetter>().SetTarget(posTwo);
            Destroy(vfxItem.gameObject, spellData.Lifetime);
        }

        private void DetectAllEnemies(Vector3 origin, IReadOnlyOffensiveData combatData, SpellData spellData)
        {
            Vector3 nextPosition = origin;

            for (int i = 0; i < spellData.Amount - 1; i++)
            {
                var hits = 
                    AreaAttacksManager.SphereOverlap
                        (nextPosition, 
                        spellData.ProjectileRadius, 
                        spellData.LayerMask).
                        ToList();
                
                if (hits.Count <= 0)
                    break;

                foreach (var enemi in enemyHits)
                {
                    hits.Remove(enemi);
                }

                hits.RemoveAll(x => x.GetComponent<IDamageable>() == null);

                int index = GetClosestIndexFromList(hits);

                if (hits.Count > 0)
                {
                    enemyHits.Add(hits[index]);
                    nextPosition = hits[index].bounds.center;
                }
            }

            if (enemyHits.Count > 1)
            {
                for (int i = 0; i < enemyHits.Count - 1; i++)
                {
                    Bind(enemyHits[i].bounds.center, enemyHits[i + 1].bounds.center);
                }

                AreaAttacksManager.DealDamageToCollisions<IDamageable>(
                    enemyHits.ToArray(),
                    combatData.AttackDamage * spellData.DamageMultiplier);
            }
        }

        private int GetClosestIndexFromList(List<Collider> hits)
        {
            int index = 0;
            float closestDistance = 0f;
            for (int j = 0; j < hits.Count; j++)
            {
                if (j == 0)
                {
                    closestDistance = Vector3.Distance(this.transform.position, hits[0].transform.position);
                    continue;
                }

                float newDistance = Vector3.Distance(hits[j].transform.position,
                    this.gameObject.transform.position);

                if (newDistance < closestDistance)
                {
                    closestDistance = newDistance;
                    index = j;
                }
            }

            return index;
        }
    }
}