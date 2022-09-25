using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

namespace CosmosDefender
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private PlayerAttributes playerAtts;

        [Header("Prefabs")] [SerializeField] private HUDAbility abilityPrefab;

        [SerializeField] private HUDAbility goddessPrefab;

        [Header("Positions")] [SerializeField] private List<RectTransform> abilityPositions;

        [SerializeField] private RectTransform goddessPosition;

        private Dictionary<CosmosSpell, HUDAbility>
            instantiatedHudAbilities = new Dictionary<CosmosSpell, HUDAbility>();

        private List<RectTransform> availablePos = new List<RectTransform>();

        // TODO Initialize with other stuff.
        private void Start()
        {
            //InstantiateAllSpells();
            Initialize();
        }

        void Initialize()
        {
            playerAtts.OnSpellAdded += AddSpell;
            playerAtts.OnSpellUpdated += UpdateSpell;

            FindObjectOfType<SpellManager>().OnSpellCasted += ApplyCooldown;

            FindObjectOfType<PlayerHealthManager>().OnDamageTaken += UpdateLife;

            FindObjectOfType<CoreHealthManager>().OnDamageTaken += UpdateCoreLife;

            FindObjectOfType<StarResourceBehavior>().OnResourceUpdated += UpdateStars;

            FindObjectOfType<GoddessResourceBehavior>().OnResourceUpdated += UpdateGoddess;

            availablePos = new List<RectTransform>();
            availablePos.AddRange(abilityPositions);
        }

        void AddSpell(CosmosSpell newSpell, bool addSpell)
        {
            if (!addSpell) return;
            
            if (availablePos.Count <= 0)
            {
                Debug.LogWarning("Cannot add visual spell. No slot available.");
            }

            var hudInstance = Instantiate(abilityPrefab, availablePos[0].position, Quaternion.identity, this.transform);
            instantiatedHudAbilities.Add(newSpell, hudInstance);
            hudInstance.UpdateVisual(newSpell.GetSpell().spellData.AbilityIcon);

            availablePos.RemoveAt(0);
        }

        void UpdateSpell(CosmosSpell spell)
        {
            instantiatedHudAbilities.TryGetValue(spell, out HUDAbility hudReference);

            if (hudReference == null) return;

            hudReference.UpdateVisual(spell.GetSpell().spellData.AbilityIcon);
            hudReference.ApplyVisualCooldown(0f);
        }

        void ApplyCooldown(ISpell spell)
        {
            var a = instantiatedHudAbilities.Keys.ToList();
            var b = a.Find(x => x.GetSpell().spellData.GetHashCode() == spell.spellData.GetHashCode());
            if (b == null) return;
            instantiatedHudAbilities.TryGetValue(b, out HUDAbility hudReference);

            if (hudReference == null) return;

            hudReference.ApplyVisualCooldown(spell.spellData.Cooldown);
        }

        [Header("Game")] [SerializeField] private ProceduralImage life;
        bool isHealthDecreasing = false;
        Coroutine healthDecreaseCoroutine;
        private float previousHealth;

        public void UpdateLife(float currentHealth, float maxHealth)
        {
            if (currentHealth >= previousHealth && !isHealthDecreasing)
            {
                life.fillAmount = currentHealth / maxHealth;
            }
            else
            {
                if (healthDecreaseCoroutine != null)
                {
                    StopCoroutine(healthDecreaseCoroutine);
                }
                healthDecreaseCoroutine = StartCoroutine(DecreaseLerpCoroutine(currentHealth / maxHealth, -50f, 0, life));
            }

            previousHealth = currentHealth;
        }

        [SerializeField] private ProceduralImage coreLife;

        public void UpdateCoreLife(float currentHealth, float maxHealth)
        {
            if (currentHealth >= previousHealth && !isHealthDecreasing)
            {
                coreLife.fillAmount = currentHealth / maxHealth;
            }
            else
            {
                if (healthDecreaseCoroutine != null)
                {
                    StopCoroutine(healthDecreaseCoroutine);
                }
                healthDecreaseCoroutine = StartCoroutine(DecreaseLerpCoroutine(currentHealth / maxHealth, -50f, 0, life));
            }

            previousHealth = currentHealth;
            coreLife.fillAmount = currentHealth / maxHealth;
        }

        IEnumerator DecreaseLerpCoroutine(float end, float speed, float delay, ProceduralImage healthImage)
        {
            isHealthDecreasing = true;
            float preChangeValue = healthImage.fillAmount;

            yield return new WaitForSeconds(delay);

            while (preChangeValue > end)
            {
                preChangeValue += Time.deltaTime * speed;
                healthImage.fillAmount = preChangeValue;

                yield return null;
            }

            healthImage.fillAmount = end;
            
            isHealthDecreasing = false;
        }

        [Header("Waves")] [SerializeField] private TMP_Text nextWaveTimer;

        [Header("Resources")] 
        [SerializeField] private TMP_Text starsText;
        
        public void UpdateStars(float value)
        {
            starsText.text = ((int)value).ToString();
        }

        public void UpdateGoddess(float value)
        {
            
        }


    }
}