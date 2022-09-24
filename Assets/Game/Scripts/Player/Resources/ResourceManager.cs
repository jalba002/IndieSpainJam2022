using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace CosmosDefender
{
    public class ResourceManager : MonoBehaviour
    {
        private Dictionary<ResourceType, IResourceModifier> resourceTable;

        private List<IResourceModifier> resourceModifiers;

        private void Awake()
        {
            resourceModifiers = new List<IResourceModifier>();
            GetComponents(resourceModifiers);
            resourceTable = resourceModifiers.ToDictionary(x => x.resourceType);
            StartCoroutine(IncreaseResourceOverTimeCoroutine());
        }

        private IEnumerator IncreaseResourceOverTimeCoroutine()
        {
            while (true)
            {
                foreach (var item in resourceTable)
                {
                    item.Value.IncreaseResourcePerSecond();
                }
                yield return new WaitForSeconds(1f);
            }
        }

        [Button("Increase Resource")]
        public void IncreaseResource(ResourceType type, float amount)
        {
            resourceTable[type].IncreaseResource(amount);
            resourceTable[type].UpdateUI();
        }

        public void DecreaseResource(ResourceType type, float amount)
        {
            resourceTable[type].OnResourceSpent(amount);
            resourceTable[type].UpdateUI();
        }

        public void SpendResource(ResourceType type, float cost)
        {
            resourceTable[type].OnResourceSpent(cost);
            resourceTable[type].UpdateUI();
        }

        public bool HasEnoughResourceToSpend(ResourceType type, float cost)
        {
            return resourceTable[type].GetCurrentResource() >= cost;
        }

        public float GetCurrentResource(ResourceType type)
        {
            return resourceTable[type].GetCurrentResource();
        }
    }

    public enum ResourceType
    {
        Stars,
        Goddess
    }
}