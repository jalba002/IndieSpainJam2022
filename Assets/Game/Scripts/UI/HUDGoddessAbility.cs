using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

namespace CosmosDefender
{
    public class HUDGoddessAbility : HUDAbility
    {
        protected override void Update()
        {
            // This does nothing.
        }

        private float maxResource;
        private float currentResource;

        public void SetResources(float current, float max)
        {
            SetResource(current);
            SetMaxResource(max);
        }

        public void SetResource(float amount)
        {
            currentResource = amount;
            UpdateValues();
        }

        public void SetMaxResource(float amount)
        {
            maxResource = amount;
            UpdateValues();
        }

        void UpdateValues()
        {
            UpdateRadial();
            abilityCooldown.fillAmount = (maxResource - currentResource) / maxResource;
        }
    }
}