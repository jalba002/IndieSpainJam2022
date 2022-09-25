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

        /// <summary>
        /// Returns false if cost is higher than the actual resource amount
        /// </summary>
        /// <param name="type"></param>
        /// <param name="cost"></param>
        /// <returns></returns>
        public bool SpendResource(ResourceType type, float cost)
        {
            if (HasEnoughResourceToSpend(type, cost))
            {
                resourceTable[type].OnResourceSpent(cost);
                resourceTable[type].UpdateUI();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasEnoughResourceToSpend(ResourceType type, float cost)
        {
            return resourceTable[type].GetCurrentResourceAmout() >= cost;
        }

        public float GetCurrentResource(ResourceType type)
        {
            return resourceTable[type].GetCurrentResourceAmout();
        }

        public ResourceData GetResourceData(ResourceType type)
        {
            return resourceTable[type].GetResourceData();
        }
    }

    public enum ResourceType
    {
        Stars,
        Goddess
    }
}