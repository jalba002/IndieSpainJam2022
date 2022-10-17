using System.Collections.Generic;
using System.Linq;
using CosmosDefender.Shop;
using Sirenix.OdinInspector;
using Unity.Collections;
using UnityEngine;

namespace CosmosDefender
{
    public abstract class BasePurchaseableModifier<T, T1, T2, T3> : ScriptableObject
        where T : PurchaseableModifierData<T1, T2, T3>
        where T1 : BaseModifier<T2, T3>
        where T2 : IModifier<T3>
    {
        //[SerializeField] private T notPurchaseableMod;
        [SerializeField, InlineEditor]
        protected List<T> modifers;

        [Space(5)]
        [SerializeField]
        private SerializableShopModifier shopData;
        
        public SerializableShopModifier ShopData { get => shopData; set => shopData = value; }

        public T GetCurrentPurchaseable() => IsModifierCompletelyPurchased() ? modifers[modifers.Count - 1] : modifers[shopData.purchasedModifiersCount];
        public T GetCurrentPurchasedItem() => modifers[shopData.purchasedModifiersCount-1];

        public bool IsModifierCompletelyPurchased() => shopData.purchasedModifiersCount == modifers.Count;
        public int GetCurrentPurchaseIndex() => shopData.purchasedModifiersCount;

        public void AddUniqueModifierToPurchase(T modifier)
        {
            if (modifers.Contains(modifier))
                return;

            modifers.Add(modifier);
        }

        public IEnumerable<T1> GetPurchasedSpells()
        {
            // TODO CHECK WITH DESIGN AND ALL.
            // Instead of sending all purchased spells, send only the last one.
            // WAS THE NEXT:
            // return modifers.Where((x, i) => i < shopData.purchasedModifiersCount).Where(x => x.CanBePurchased).Select(x => x.modifier);
            // Now this returns the highest tier modifier that has been bought.
            return modifers.Where((x, i) => i == shopData.purchasedModifiersCount-1).Where(x => x.CanBePurchased).Select(x => x.modifier);
        }
        public void Purchase(T modifiedToPurchase)
        {
            if (!modifiedToPurchase.CanBePurchased)
                return;

            if (modifers.Contains(modifiedToPurchase))
                shopData.purchasedModifiersCount++;
        }
    }
}