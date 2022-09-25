using TMPro;
using UnityEngine;

namespace CosmosDefender
{
    public class StarResourceBehavior : MonoBehaviour, IResourceModifier
    {
        [SerializeField] private ResourceConfig resourceData;
        [SerializeField] private TextMeshProUGUI resourceText;

        private ResourceData starResourceData;
        public ResourceType resourceType => resourceData.ResourceType;

        private void Start()
        {
            starResourceData = resourceData.baseResource;
            starResourceData.CurrentResource = starResourceData.StartingResource;
            UpdateUI();
        }

        public void UpdateUI()
        {
            resourceText.text = "Stars: " + (int)starResourceData.CurrentResource;
        }

        public float GetCurrentResourceAmout()
        {
            return starResourceData.CurrentResource;
        }

        public void OnResourceSpent(float cost)
        {
            starResourceData.CurrentResource -= cost;
            starResourceData.CurrentResource = Mathf.Clamp(starResourceData.CurrentResource, 0, starResourceData.MaxResource);
            UpdateUI();
        }

        public void IncreaseResource(float amount)
        {
            starResourceData.CurrentResource += amount;
            starResourceData.CurrentResource = Mathf.Clamp(starResourceData.CurrentResource, 0, starResourceData.MaxResource);
            UpdateUI();
        }

        public void IncreaseResourcePerSecond()
        {
            IncreaseResource(starResourceData.ResourceOverTime);
        }

        public ResourceData GetResourceData()
        {
            return starResourceData;
        }
    }
}