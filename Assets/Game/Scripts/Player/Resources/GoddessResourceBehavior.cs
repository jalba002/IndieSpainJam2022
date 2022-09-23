using UnityEngine;

namespace CosmosDefender
{
    public class GoddessResourceBehavior : MonoBehaviour, IResourceModifier
    {
        [SerializeField] private ResourceData resourceData;

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
            throw new System.NotImplementedException();
        }

        public void DecreaseResource(float value)
        {
            throw new System.NotImplementedException();
        }
    }
}