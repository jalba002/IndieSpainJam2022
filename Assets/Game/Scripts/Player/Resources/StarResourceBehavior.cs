using TMPro;
using UnityEngine;

namespace CosmosDefender
{
    public class StarResourceBehavior : MonoBehaviour, IResourceModifier
    {
        [SerializeField] private ResourceData resourceData;
        [SerializeField] private TextMeshProUGUI resourceText;
        [SerializeField] private ResourceData data;

        private float currentResource = 0f;

        private void Start()
        {
            currentResource = resourceData.StartingResource;
            resourceText.text = "Stars: " + currentResource;
        }

        public bool HasEnoughResource(ResourceData data, float cost)
        {
            if (data.ResourceType != ResourceType.Stars)
            {
                return false;
            }

            return currentResource >= cost;
        }

        public ResourceType GetResourceType()
        {
            return resourceData.ResourceType;
        }

        public void IncreaseResource(ResourceData data, float amount)
        {
            currentResource += amount;
            UpdateUI();
        }

        public void DecreaseResource(ResourceData data, float amount)
        {
            currentResource -= amount;
            UpdateUI();
        }

        public void UpdateUI()
        {
            resourceText.text = "Stars: " + data.CurrentResource;
        }

        public float GetCurrentResource()
        {
            return currentResource;
        }
    }
}