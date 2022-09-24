using TMPro;
using UnityEngine;

namespace CosmosDefender
{
    public class GoddessResourceBehavior : MonoBehaviour, IResourceModifier
    {
        [SerializeField] private ResourceData resourceData;
        [SerializeField] private TextMeshProUGUI resourceText;

        private float currentResource = 0f;

        private void Start()
        {
            currentResource = resourceData.StartingResource;
            UpdateUI();
        }

        public ResourceType GetResourceType()
        {
            return resourceData.ResourceType;
        }

        public bool HasEnoughResource(ResourceData data, float cost)
        {
            if (data.ResourceType != ResourceType.Stars)
            {
                return false;
            }

            return currentResource >= cost;
        }

        public void IncreaseResource(ResourceData data, float amount)
        {
            throw new System.NotImplementedException();
        }

        public void IncreaseResource(float amount)
        {
            resourceData.CurrentResource += amount;
            resourceData.CurrentResource = Mathf.Clamp(resourceData.CurrentResource, 0, resourceData.MaxResource);
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
            resourceText.text = "Goddess: " + resourceData.CurrentResource;
        }
    }
}