using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CosmosDefender
{
    public class CoreHealthManager : HealthManager
    {
        [SerializeField] private Animator animator;
        public override void Die()
        {
            // base.Die();
            // Play animation? Or don't.
            animator.SetTrigger("Die");
        }

        public override void DamageFeedback()
        {
            // base.DamageFeedback();
            animator.SetTrigger("Damaged");
            animator.SetFloat("Life", currentHealth / MaxHealth);
        }
    }
}