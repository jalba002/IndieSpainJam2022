using System;
using System.Collections;
using System.Collections.Generic;
using CosmosDefender.Bullets;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace CosmosDefender
{
    public class EnemyAI : MonoBehaviour, ISpellCaster
    {
        [SerializeField] private EnemyData data;

        private Transform chasingTarget;

        [SerializeField] private BaseSpell attack;

        [SerializeField] private Transform attackOrigin;

        [SerializeField] private Animator _animator;

        public GameObject GameObject => this.gameObject;

        public Animator Animator => _animator;
        public Transform CastingPoint => attackOrigin;
        public void SetAnimationTrigger(string triggerName) => Animator.SetTrigger(triggerName);
        
        [Button]
        public void Attack()
        {
            attack.UpdateCurrentData();
            attack.Cast(attackOrigin.position, transform.forward, transform.rotation, data,this);
        }

        [Button]
        public void StopAttack()
        {
            // Ranged usually stops attacking.
        }

        private void Update()
        {
            UpdateAI();
        }

        // 
        private void UpdateAI()
        {
            // Ai goes for the nexus
            // if detects the player, go for him. Until he goes really far.
            // if in range of the player and attack, ATTACK.
            // If out of range of attack, STOP ATTACK.
            if (chasingTarget)
            {
                // Set the NavMesh pos to the player. or a nearby valid position. Over time maybe.
            }
        }

        public void NoticeAI()
        {
            // When the player gets nearby it will notice the enemies around him.
        }
    }
}