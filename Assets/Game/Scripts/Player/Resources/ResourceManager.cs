using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CosmosDefender
{
    public class ResourceManager : MonoBehaviour
    {
        [SerializeField]
        private List<IResourceModifier> resourceModifiers = new List<IResourceModifier>();

        [SerializeField]
        private List<ResourceData> resourceDatas = new List<ResourceData>();

        private void Awake()
        {
            GetComponents(resourceModifiers);
        }

        void Start()
        {
            foreach (var item in resourceDatas)
            {
                item.Initialize();
            }
        }

        [Button("Increase Resource")]
        public void IncreaseResource(ResourceData data, float amount)
        {
            data.IncreaseResource(amount);
        }

        public void DecreaseResource(ResourceData data, float amount)
        {
            data.DecreaseResource(amount);
        }

        public bool HasEnoughResource(ResourceData data, float cost)
        {
            return data.HasEnoughResource(cost);
        }

        public void UpdateUI()
        {
            foreach (var item in resourceModifiers)
            {
                item.UpdateUI();
            }
        }
    }

    public enum ResourceType
    {
        Stars,
        Goddess
    }
}