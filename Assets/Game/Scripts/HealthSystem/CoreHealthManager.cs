using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace CosmosDefender
{
    public class CoreHealthManager : HealthManager
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float healthRegeneration;

        [Header("Sounds")] [SerializeField] private StudioEventEmitter DamagedSound;
        [SerializeField] private StudioEventEmitter DeathSound;

        public override void Start()
        {
            base.Start();

            StartCoroutine(HealthRegenerationCoroutine());
        }

        public override void Die()
        {
            // This stops from bugging or replaying anims.
            base.Die();
            animator.SetTrigger("Die");
            
            // Destroy the core alerter.
            GetComponent<EnemyAlerter>().enabled = false;
            GameManager.Instance.EndGame(false);
            DeathSound.Play();
        }

        public override void DamageFeedback()
        {
            base.DamageFeedback();
            animator.SetTrigger("Damaged");
            animator.SetFloat("Life", currentHealth / MaxHealth);
            // SOUND?
            DamagedSound.PlayInstance();
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