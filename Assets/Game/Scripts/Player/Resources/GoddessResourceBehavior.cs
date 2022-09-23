using TMPro;
using UnityEngine;

namespace CosmosDefender
{
    public class GoddessResourceBehavior : MonoBehaviour, IResourceModifier
    {
        [SerializeField] private ResourceData resourceData;
        [SerializeField] private TextMeshProUGUI resourceText;
        [SerializeField] private ResourceData data;

        private float currentResource = 0f;

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

        public void DecreaseResource(ResourceData data, float amount)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateUI()
        {
            resourceText.text = "Goddess: " + data.CurrentResource;
        }
    }
}