using System;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace CosmosDefender
{
    public class EnemyAI : MonoBehaviour, ISpellCaster
    {
        [SerializeField] private EnemyData data;

        private Transform chasingTarget;

        [SerializeField] private BaseSpell attack;

        [SerializeField] private Transform attackOrigin;

        [SerializeField] private Animator _animator;

        [SerializeField] private NavMeshAgent agent;

        private float timeToMove = 0f;

        public GameObject GameObject => this.gameObject;

        public Animator Animator => _animator;
        public Transform CastingPoint => attackOrigin;
        public void SetAnimationTrigger(string triggerName) => Animator.SetTrigger(triggerName);

        private float timeToAttack = 0f;

        [SerializeField] private PathFollower path;

        private void Start()
        {
            agent.updateRotation = true;
            agent.speed = data.Speed;
            agent.stoppingDistance = data.attackRange * 0.25f;
            //agent.isStopped = true;
            //agent.updateRotation = false;
        }

        private void Update()
        {
            Animator.SetFloat("Speed", agent.isStopped ? 0f : agent.velocity.magnitude / agent.speed);
            UpdateAI();
        }

        private void UpdateAI()
        {
            // Ai goes for the nexus
            // if detects the player, go for him. Until he goes really far.
            // if in range of the player and attack, ATTACK.
            // If out of range of attack, STOP ATTACK.

            if (chasingTarget != null)
            {
                Vector3 enemyDirection = (chasingTarget.position - transform.position);
                if (Vector3.Distance(transform.position, chasingTarget.position) >= data.attackRange)
                {
                    Vector3 attackPosition = (transform.position - chasingTarget.position).normalized;
                    Vector3 finalPos = chasingTarget.position + attackPosition * (data.attackRange);
                    agent.SetDestination(finalPos);
                }

                // Rotate here to face player constantly. Use the rotation speed or some.
                // Does the agent rotate towards the player alone?

                if (enemyDirection.magnitude > data.AggroRange)
                {
                    chasingTarget = null;
                    //path.ReturnToPath();
                }
                else if (enemyDirection.magnitude < data.attackRange)
                {
                    // Try to attack.
                    // FORCE ROTATION IF DOESNT WORK
                    AttackAndCooldown();
                }
            }
        }

        public void AttackAndCooldown()
        {
            // If the skill is not on cooldown, then attack!
            if (!(Time.time > timeToAttack)) return;
            
            Attack();
            // apply cooldown.
            timeToAttack = Time.time + attack.spellData.Cooldown;
            CronoScheduler.Instance.ScheduleForTime(attack.spellData.Cooldown, () => { agent.updatePosition = true; });
        }

        [Button]
        void Attack()
        {
            attack.UpdateCurrentData();
            // Raycast anyway to rotate to player.
            Quaternion targetRotation = transform.rotation;
            if (chasingTarget != null)
            {
                targetRotation = Quaternion.LookRotation((
                    (chasingTarget.transform.position + Vector3.up) -
                    attackOrigin.transform.position).normalized);
            }

            attack.Cast(attackOrigin.position, transform.forward, targetRotation, data, this);
            agent.updatePosition = false;
        }

        public void Alert(Transform who)
        {
            // When the player gets nearby it will notice the enemies around him.
            chasingTarget = who;
            //agent.isStopped = false;
        }
    }
}