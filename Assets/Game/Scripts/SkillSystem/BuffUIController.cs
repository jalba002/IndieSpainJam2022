using System.Collections.Generic;
using UnityEngine;

namespace CosmosDefender
{
    public class BuffUIController : MonoBehaviour
    {
        [SerializeField]
        private BuffImage buffPrefab;
        [SerializeField]
        private PlayerAttributes playerAttributes;
        [SerializeField]
        private RectTransform gridParent;

        private List<BuffImage> buffsList;
        private List<BuffImage> pool;

        public void Awake()
        {
            buffsList = new List<BuffImage>();
            pool = new List<BuffImage>();
            playerAttributes.onModifiersUpdated += OnModifiersUpdated;
            OnModifiersUpdated(playerAttributes.RequestBuffs());
        }

        private void OnModifiersUpdated(IReadOnlyList<IBuffProvider> modifiers)
        {
            ResizeBuffs(modifiers.Count);
            for (int i = 0; i < modifiers.Count; i++)
                buffsList[i].Show(modifiers[i].BuffSprite);
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
            if (pool.Count == 0)
                pool.Add(Instantiate(buffPrefab, gridParent));

            var item = pool[0];
            item.gameObject.SetActive(true);
            return item;
        }
    }
}