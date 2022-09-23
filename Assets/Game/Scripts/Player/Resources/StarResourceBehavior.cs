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
            resourceText.text = "Stars: " + currentResource;
        }

        public void CanUseResource(float value)
        {
            throw new System.NotImplementedException();
        }

        public ResourceType GetResourceType()
        {
            return resourceData.ResourceType;
        }

        public void IncreaseResource(float value)
        {
            currentResource += value;
            UpdateResources();
        }

        public void DecreaseResource(float value)
        {
            currentResource -= value;
            UpdateResources();
        }

        public void UpdateResources()
        {
            currentResource = Mathf.Clamp(currentResource, 0, resourceData.MaxResource);
            resourceText.text = "Stars: " + currentResource;
        }

        public float GetCurrentResource()
        {
            return currentResource;
        }
    }
}