using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CosmosDefender
{
    public class ResourceManager : MonoBehaviour
    {
        [SerializeField]
        private List<IResourceModifier> resourceModifiers = new List<IResourceModifier>();
        
        [SerializeField]
        private Dictionary<ResourceType, IResourceModifier> ResourcesDictionary = new Dictionary<ResourceType, IResourceModifier>();

        [SerializeField]
        private StarResourceBehavior starsResourceModifier;

        [SerializeField]
        private GoddessResourceBehavior goddessResourceModifier;

        private void Awake()
        {
            starsResourceModifier = GetComponent<StarResourceBehavior>();
            goddessResourceModifier = GetComponent<GoddessResourceBehavior>();
        }

        private void Start()
        {
            //GetComponents(resourceModifiers);
            //foreach (var item in resourceModifiers)
            //{
            //    ResourcesDictionary.Add(item.GetResourceType(), item);
            //}
        }

        void Update()
        {

        }

        [Button("Increase Resource")]
        public void IncreaseResource(float value)
        {
            starsResourceModifier.IncreaseResource(value);
        }
    }

    public enum ResourceType
    {
        Stars,
        Goddess
    }
}