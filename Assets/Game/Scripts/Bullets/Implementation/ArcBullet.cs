using System.Collections.Generic;
using System.Linq;
using CosmosDefender.Projectiles;
using UnityEngine;

namespace CosmosDefender.Bullets.Implementation
{
    public class ArcBullet : BaseBullet
    {
        public BaseProjectile prefab;

        public LayerMask layerMask;

        private List<Collider> enemyHits = new List<Collider>();

        private IReadOnlyOffensiveData m_CombatData;

        private SpellData m_SpellData;

        [Header("Components")]
        [SerializeField]
        private Collider m_Collider;

        [SerializeField] private Rigidbody _rigidbody;

        public override void InstantiateBullet(Vector3 origin, Vector3 forward, Quaternion rotation,
            IReadOnlyOffensiveData combatData, SpellData spellData)
        {
            base.InstantiateBullet(origin, forward, rotation, combatData, spellData);

            m_SpellData = spellData;

            m_CombatData = combatData;

            _rigidbody.velocity = forward * spellData.Speed;

            CronoScheduler.Instance.ScheduleForTime(spellData.Amount * spellData.ProjectileDelay + 1f,
                () => { Destroy(this.gameObject); });
        }

        private void DetectAllEnemies(Vector3 origin, IReadOnlyOffensiveData combatData, SpellData spellData)
        {
            // When first casting this spell, detect this first enemy, then cast spheres and gather the closest enemy. 
            // Then repeat for X times or when no enemies are detected.

            //enemyHits = new List<Collider>();
            Vector3 nextPosition = origin;
            float realRadius = spellData.ProjectileRadius * .2f;
            // Amount is the max amount of enemies that will be hit.
            for (int i = 0; i < spellData.Amount - 1; i++)
            {
                // TODO Could cause issues.
                var hits = AreaAttacksManager.SphereOverlap(nextPosition, realRadius, layerMask).ToList();
                if (hits.Count <= 0)
                {
                    Debug.Log("No more enemies detected.");
                    break;
                }
                else
                {
                    foreach (var enemi in enemyHits)
                    {
                        hits.Remove(enemi);
                    }

                    hits.RemoveAll(x => x.GetComponent<IDamageable>() == null);

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

                    if (hits.Count > 0)
                    {
                        enemyHits.Add(hits[index]);
                        nextPosition = hits[index].gameObject.transform.position;
                    }
                }
            }

            // string debug = $"Arc detected {enemyHits.Count} damageable objects.\n";
            // foreach (var enemy in enemyHits)
            // {
            //     debug += enemy + "\n";
            // }
            //
            // Debug.Log(debug);

            for (int i = 0; i < enemyHits.Count - 1; i++)
            {
                Debug.DrawLine(enemyHits[i].transform.position, enemyHits[i + 1].transform.position, Color.blue, 5f);
            }

            AreaAttacksManager.DealDamageToCollisions<IDamageable>(enemyHits.ToArray(), m_CombatData.AttackDamage * m_SpellData.DamageMultiplier);
        }

        private void OnTriggerEnter(Collider other)
        {
            var a = other.gameObject.GetComponent<IDamageable>();
            if (a != null)
            {
                // Collider disabled.
                m_Collider.enabled = false;
                _rigidbody.velocity = Vector3.zero;
                // Mesh renderer disabled too?
                enemyHits.Add(other);
                DetectAllEnemies(other.gameObject.transform.position, m_CombatData, m_SpellData);
            }
        }
    }
}