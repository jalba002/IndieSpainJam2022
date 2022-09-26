using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CosmosDefender
{
    public enum CooldownVisuals
    {
        Radial,
        Vertical
    }
    public class HUDAbility : MonoBehaviour
    {
        [SerializeField] private Image abilityImageComponent;
        
        [SerializeField] protected Image abilityCooldown;
        
        [SerializeField] private TMP_Text numberAbilityCooldown;

        [SerializeField] private CooldownVisuals visualMode;

        private float maxCooldown;
        private float currentCooldown;

        protected void UpdateRadial()
        {
            abilityCooldown.fillMethod = visualMode == CooldownVisuals.Radial
                ? Image.FillMethod.Radial360
                : Image.FillMethod.Vertical;
             abilityCooldown.fillOrigin = visualMode == CooldownVisuals.Radial
                 ? 2
                 : 1;
        }

        public void UpdateCooldownValues(float cd, float maxcd)
        {
            this.currentCooldown = cd;
            this.maxCooldown = maxcd;
            UpdateRadial();
            UpdateFillAmount();
        }

        public void UpdateMaxCooldown(float maxCooldown)
        {
            UpdateRadial();
            //this.currentCooldown = maxCooldown;
            this.maxCooldown = maxCooldown;
            UpdateFillAmount();
        }

        public void SetCooldown(float cooldown)
        {
            currentCooldown = cooldown;
            UpdateFillAmount();
        }
        
        public void UpdateVisual(Sprite AbilityIcon)
        {
            // Only gather visuals.
            abilityImageComponent.sprite = AbilityIcon;
        }

        protected virtual void Update()
        {
            if (currentCooldown > 0f)
            {
                currentCooldown -= Time.deltaTime;
                currentCooldown = MathF.Max(0f, currentCooldown);
                UpdateFillAmount();
            }
        }

        private void UpdateFillAmount()
        {
            abilityCooldown.fillAmount = (float)((currentCooldown * 100f) / (maxCooldown * 100f));
            if (currentCooldown <= 0f)
                numberAbilityCooldown.text = "";
            else
                numberAbilityCooldown.text = currentCooldown > 1f ? ((int) currentCooldown).ToString(): currentCooldown.ToString("F1");
        }
    }
}