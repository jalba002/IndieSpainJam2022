using TMPro;
using UnityEngine;

namespace CosmosDefender
{
    public class GoddessResourceBehavior : MonoBehaviour, IResourceModifier
    {
        [SerializeField] private ResourceConfig resourceData;
        [SerializeField] private TextMeshProUGUI resourceText;

        private ResourceData goddessResourceData;
        public ResourceType resourceType => resourceData.ResourceType;

        private void Start()
        {
            goddessResourceData = resourceData.baseResource;
            goddessResourceData.CurrentResource = goddessResourceData.StartingResource;
            UpdateUI();
        }

        public void UpdateUI()
        {
            resourceText.text = "Goddess: " + (int)goddessResourceData.CurrentResource;
        }

        public float GetCurrentResource()
        {
            return goddessResourceData.CurrentResource;
        }

        public void OnResourceSpent(float cost)
        {
            goddessResourceData.CurrentResource += cost;
            goddessResourceData.CurrentResource = Mathf.Clamp(goddessResourceData.CurrentResource, 0, goddessResourceData.MaxResource);
            UpdateUI();
        }

        public void IncreaseResource(float amount)
        {
            goddessResourceData.CurrentResource += amount;
            goddessResourceData.CurrentResource = Mathf.Clamp(goddessResourceData.CurrentResource, 0, goddessResourceData.MaxResource);
            UpdateUI();
        }

        public void IncreaseResourcePerSecond()
        {
            IncreaseResource(goddessResourceData.ResourceOverTime);
        }
    }
}