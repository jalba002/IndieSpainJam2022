using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CosmosDefender {
    public class PlayerHealthManager : HealthManager
    {
        public float InvulnerableTime = 1f;
        public ScreenShake mainCameraShake;

        public float CameraShakeDuration = 0.15f;
        public float CameraShakeIntensity = 5f;

        public Slider HealthSlider;
        public Slider HealthSliderYellow;

        private Coroutine healthBarLerpCoroutine;
        private Coroutine healthBarYellowLerpCoroutine;
        [SerializeField] private Animator animator;
        [SerializeField] private PlayerAttributes playerAttributes;

        private void Awake()
        {
            //inputs = GetComponent<PlayerInputsManager>();
        }

        public override void Start()
        {
            MaxHealth = playerAttributes.DefensiveData.MaxHealth;

            base.Start();

            StartCoroutine(HealthRegenerationCoroutine());
        }

        private IEnumerator HealthRegenerationCoroutine()
        {
            while (true)
            {
                if(currentHealth < MaxHealth)
                {
                    IncreaseHealth(playerAttributes.DefensiveData.HealthRegeneration);
                    OnDamageTaken?.Invoke(currentHealth, MaxHealth);
                    //if (!isUpdatingHealthbar)
                    //    IncreaseHealthSmoothUpdateUI(currentHealth, 10f);
                }
                yield return new WaitForSeconds(1f);
            }
        }

        public override void Die()
        {
            //mainCameraShake.CameraShake(CameraShakeDuration, CameraShakeIntensity);
            //DecreaseHealthSmoothUpdateUI(currentHealth, -50f);
            //inputs.DisableInputs();
            OnDamageTaken?.Invoke(currentHealth, MaxHealth);
            StartCoroutine(DeathCoroutine());
            animator.SetTrigger("Death");
            SetInvulnerableState(true);
            // AVOID MOVEMENT
            GetComponent<PlayerMovementController>().enabled = false;
            GetComponent<SpellManager>().enabled = false;
        }

        public override void DamageFeedback()
        {
            base.DamageFeedback();
            InvulnerableOverTime(InvulnerableTime);
            mainCameraShake.CameraShake(CameraShakeDuration, CameraShakeIntensity);
            //DecreaseHealthSmoothUpdateUI(currentHealth, -50f);
        }

        private void DecreaseHealthSmoothUpdateUI(float healthRemaining, float duration)
        {
            if (healthBarLerpCoroutine != null)
            {
                StopCoroutine(healthBarLerpCoroutine);
            }
            if (healthBarYellowLerpCoroutine != null)
            {
                StopCoroutine(healthBarYellowLerpCoroutine);
            }

            //isUpdatingHealthbar = true;
            healthBarLerpCoroutine = StartCoroutine(DecreaseLerpCoroutine(healthRemaining, duration, 0, HealthSlider));
            healthBarYellowLerpCoroutine = StartCoroutine(DecreaseLerpCoroutine(healthRemaining, -25f, 1f, HealthSliderYellow));
        }

        private void IncreaseHealthSmoothUpdateUI(float healthRemaining, float duration)
        {
            if (healthBarLerpCoroutine != null)
            {
                StopCoroutine(healthBarLerpCoroutine);
            }
            healthBarLerpCoroutine = StartCoroutine(IcreaseLerpCoroutine(healthRemaining, duration, 0, HealthSlider));
        }

        IEnumerator DecreaseLerpCoroutine(float end, float speed, float delay, Slider healthSlider)
        {
            float preChangeValue = healthSlider.value;

            yield return new WaitForSeconds(delay);

            while (preChangeValue > end)
            {
                preChangeValue += Time.deltaTime * speed;
                healthSlider.value = preChangeValue;

                yield return null;
            }

            healthSlider.value = currentHealth;
            if (healthSlider == HealthSliderYellow)
            {
                //isUpdatingHealthbar = false;
            }
        }

        IEnumerator IcreaseLerpCoroutine(float end, float speed, float delay, Slider healthSlider)
        {
            float preChangeValue = healthSlider.value;

            yield return new WaitForSeconds(delay);

            while (preChangeValue < end)
            {
                preChangeValue += Time.deltaTime * speed;
                healthSlider.value = preChangeValue;

                yield return null;
            }

            healthSlider.value = currentHealth;
            HealthSliderYellow.value = currentHealth;
        }

        IEnumerator DeathCoroutine()
        {
            yield return new WaitForSeconds(1f);
            //LoadingScreen.FadeIn();
            yield return new WaitForSeconds(1f);
            GameManager.Instance.EndGame(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "MapLimit")
            {
                TakeDamage(25f);
            }
        }
    }
}
