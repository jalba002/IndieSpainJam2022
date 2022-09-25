using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CosmosDefender
{
    public class CoreHealthManager : HealthManager
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float healthRegeneration;

        public override void Start()
        {

            base.Start();

            StartCoroutine(HealthRegenerationCoroutine());
        }

        public override void Die()
        {
            // base.Die();
            // Play animation? Or don't.
            animator.SetTrigger("Die");
            GameManager.Instance.EndGame(false);
        }

        public override void DamageFeedback()
        {
            // base.DamageFeedback();
            animator.SetTrigger("Damaged");
            animator.SetFloat("Life", currentHealth / MaxHealth);
        }

        private IEnumerator HealthRegenerationCoroutine()
        {
            while (true)
            {
                if (currentHealth < MaxHealth)
                {
                    IncreaseHealth(healthRegeneration);
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
}