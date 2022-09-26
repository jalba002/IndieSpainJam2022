using System;
using System.Collections;
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

        private Transform alerterTarget;

        [SerializeField] private BaseSpell attack;

        [SerializeField] private Transform attackOrigin;

        [SerializeField] private Animator _animator;

        [SerializeField] private NavMeshAgent agent;

        public GameObject GameObject => this.gameObject;

        public Animator Animator => _animator;
        public Transform CastingPoint => attackOrigin;
        public void SetAnimationTrigger(string triggerName) => Animator.SetTrigger(triggerName);

        private float timeToAttack = 0f;

        [SerializeField] private PathFollower path;
        private EnemyAlerter currentAlerter;
        private bool isAttacking = false;
        private bool wasChasing = false;

        private float loseAggroRange;
        private Coroutine attackCooldownCoroutine;

        private void Start()
        {
            //We increase aggro range in case the nav mesh moves this character away from the alerter, this way we prevent ReturnToPath spam
            loseAggroRange = data.AggroRange + 1.5f;
        }

        private void Update()
        {
            UpdateAI();
        }

        private void UpdateAI()
        {
            if (isAttacking)
                return;
            // Ai goes for the nexus
            // if detects the player, go for him. Until he goes really far.
            // if in range of the player and attack, ATTACK.
            // If out of range of attack, STOP ATTACK.
            _animator.SetFloat("Speed", agent.isStopped ? 0f : agent.velocity.magnitude / agent.speed);

            if (alerterTarget != null)
            {
                // Rotate here to face player constantly. Use the rotation speed or some.
                // Does the agent rotate towards the player alone? Maybe
                Vector3 alerterDirection = (alerterTarget.position - transform.position);
                if (alerterDirection.magnitude <= data.attackRange)
                {
                    agent.isStopped = true;
                    // Try to attack.
                    // FORCE ROTATION IF DOESNT WORK
                    if (!IsSkillOnCooldown())
                    {
                        AttackAndCooldown();
                    }
                    else
                    {
                        transform.LookAt(alerterTarget);
                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                    }
                }
                else if(alerterDirection.magnitude > data.attackRange && alerterDirection.magnitude <= loseAggroRange)
                {
                    agent.destination = alerterTarget.position;
                }
                else if (alerterDirection.magnitude > loseAggroRange)
                {
                    alerterTarget = null;
                }
            }
            else if (wasChasing)
            {
                if (isAttacking == false)
                {
                    wasChasing = false;
                    path.ReturnToPath();
                }
            }
        }

        public void AttackAndCooldown()
        {
            Attack();
            // apply cooldown.
            timeToAttack = Time.time + attack.spellData.Cooldown;
            attackCooldownCoroutine = CronoScheduler.Instance.ScheduleForTime(attack.spellData.ActiveDuration, () => 
            { 
                isAttacking = false;
                agent.isStopped = false;
            });
        }

        public void Death()
        {
            if(attackCooldownCoroutine != null)
                StopCoroutine(attackCooldownCoroutine);

            attack.StopCast();
        }

        bool IsSkillOnCooldown()
        {
            return Time.time < timeToAttack;
        }

        [Button]
        void Attack()
        {
            transform.LookAt(alerterTarget);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

            isAttacking = true;
            agent.isStopped = true;
            attack.UpdateCurrentData();
            // Raycast anyway to rotate to player.
            Quaternion targetRotation = transform.rotation;
            if (alerterTarget != null)
            {
                targetRotation = Quaternion.LookRotation((
                    (alerterTarget.transform.position + Vector3.up) -
                    attackOrigin.transform.position).normalized);
            }

            attack.Cast(attackOrigin.position, transform.forward, targetRotation, data, this);
        }

        public void Alert(EnemyAlerter newAlerter)
        {
            if (currentAlerter != null)
            {
                if (currentAlerter.priority >= newAlerter.priority)
                    return;
            }
            
            // When the player gets nearby it will notice the enemies around him.
            Vector3 alerterDirection = (newAlerter.transform.position - transform.position);
            if (alerterDirection.magnitude <= data.AggroRange)
            {
                alerterTarget = newAlerter.transform;
                wasChasing = true;
            }
        }

        private bool IsPathIsValid(Vector3 position)
        {
            NavMeshPath navMeshPath = new NavMeshPath();
            agent.CalculatePath(currentAlerter.transform.position, navMeshPath);

            if (navMeshPath.status != NavMeshPathStatus.PathComplete)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}