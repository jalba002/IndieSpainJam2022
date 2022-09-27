using System.Collections.Generic;
using UnityEngine;

namespace CosmosDefender
{
    public class BuffUIController : MonoBehaviour
    {
        [SerializeField] private BuffImage buffPrefab;
        [SerializeField] private PlayerAttributes playerAttributes;
        [SerializeField] private RectTransform gridParent;

        private List<BuffImage> buffsList;
        private List<BuffImage> pool;

        public void Awake()
        {
            buffsList = new List<BuffImage>();
            pool = new List<BuffImage>();
            playerAttributes.onModifiersUpdated += OnModifiersUpdated;
            OnModifiersUpdated(playerAttributes.RequestBuffs());
        }

        private void OnDestroy()
        {
            playerAttributes.onModifiersUpdated -= OnModifiersUpdated;
        }

        private void OnModifiersUpdated(IReadOnlyList<IBuffProvider> modifiers)
        {
            ResizeBuffs(modifiers.Count);
            //Debug.Log($"Hay {modifiers.Count} mods. Una lista de {buffsList.Count}");
            for (int i = 0; i < modifiers.Count; i++)
            {
                Debug.Log($"Creating buff with {modifiers[i].Tier}");
                buffsList[i].Show(modifiers[i].BuffSprite, modifiers[i].Tier);
            }
        }

        private void ResizeBuffs(int length)
        {
            while (length < buffsList.Count)
            {
                int lastIndex = buffsList.Count - 1;
                ReturnToPool(buffsList[lastIndex]);
                buffsList.RemoveAt(lastIndex);
            }

            while (buffsList.Count < length)
            {
                buffsList.Add(GetFromPool());
            }
        }

        private void ReturnToPool(BuffImage image)
        {
            image.gameObject.SetActive(false);
            pool.Add(image);
        }

        private BuffImage GetFromPool()
        {
            var poolItem = Instantiate(buffPrefab, gridParent);
                    
            pool.Add(poolItem);

            //var item = pool[0];
            poolItem.gameObject.SetActive(true);
            return poolItem;
        }
    }
}