using TMPro;
using UnityEngine;

namespace CosmosDefender
{
    public class StarResourceBehavior : MonoBehaviour, IResourceModifier
    {
        [SerializeField] private ResourceData resourceData;
        [SerializeField] private TextMeshProUGUI resourceText;

        private float currentResource = 0f;

        private void Start()
        {
            currentResource = resourceData.StartingResource;
            UpdateUI();
        }

        public bool HasEnoughResource(ResourceData data, float cost)
        {
            if (data.ResourceType != ResourceType.Stars)
            {
                return false;
            }

            return resourceData.CurrentResource >= cost;
        }

        public ResourceType GetResourceType()
        {
            return resourceData.ResourceType;
        }

        public void IncreaseResource(ResourceData data, float amount)
        {
            resourceData.CurrentResource += amount;
            data.CurrentResource = Mathf.Clamp(data.CurrentResource, 0, data.MaxResource);
            UpdateUI();
        }

        public void DecreaseResource(ResourceData data, float amount)
        {
            resourceData.CurrentResource -= amount;
            resourceData.CurrentResource = Mathf.Clamp(resourceData.CurrentResource, 0, resourceData.MaxResource);
            UpdateUI();
        }

        public void UpdateUI()
        {
            resourceText.text = "Stars: " + resourceData.CurrentResource;
        }

        public float GetCurrentResource()
        {
            return currentResource;
        }
    }
}