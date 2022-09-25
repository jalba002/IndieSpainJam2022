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

        private float timeToMove = 0f;

        public GameObject GameObject => this.gameObject;

        public Animator Animator => _animator;
        public Transform CastingPoint => attackOrigin;
        public void SetAnimationTrigger(string triggerName) => Animator.SetTrigger(triggerName);

        private float timeToAttack = 0f;

        [SerializeField] private PathFollower path;
        private EnemyAlerter currentAlerter;
        private bool isAttacking = false;
        private bool wasChasing = false;

        private void Update()
        {
            UpdateAI();
        }

        private void UpdateAI()
        {
            // Ai goes for the nexus
            // if detects the player, go for him. Until he goes really far.
            // if in range of the player and attack, ATTACK.
            // If out of range of attack, STOP ATTACK.

            if (alerterTarget != null)
            {
                // Rotate here to face player constantly. Use the rotation speed or some.
                // Does the agent rotate towards the player alone? Maybe
                Vector3 alerterDirection = (alerterTarget.position - transform.position);
                if (alerterDirection.magnitude <= data.attackRange)
                {
                    // Try to attack.
                    // FORCE ROTATION IF DOESNT WORK
                    Debug.Log("Skill is " + (IsSkillOnCooldown()? "" : "not ")+ "on cooldown");
                    if (IsSkillOnCooldown())
                    {
                        agent.destination = alerterTarget.position;
                    }
                    else
                    {
                        AttackAndCooldown();
                    }
                }
                else if(alerterDirection.magnitude > data.AggroRange)
                {
                    agent.destination = alerterTarget.position;
                }
                else if (alerterDirection.magnitude <= data.AggroRange)
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
            CronoScheduler.Instance.ScheduleForTime(attack.spellData.ActiveDuration, () => 
            { 
                isAttacking = false;
            });
        }

        bool IsSkillOnCooldown()
        {
            return Time.time < timeToAttack;
        }

        [Button]
        void Attack()
        {
            isAttacking = true;
            path.StopFollowingPath();
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

        public void Alert(EnemyAlerter alerter)
        {
            if (currentAlerter?.priority >= alerter.priority)
                return;
            // When the player gets nearby it will notice the enemies around him.
            alerterTarget = alerter.transform;
            //agent.isStopped = false;
        }
    }
}