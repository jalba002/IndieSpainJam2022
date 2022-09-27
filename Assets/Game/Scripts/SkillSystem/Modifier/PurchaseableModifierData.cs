using Sirenix.OdinInspector;
using UnityEngine;

namespace CosmosDefender
{
    public class PurchaseableModifierData<T, T1, T2> : ScriptableObject, IShopDisplayer where T : BaseModifier<T1, T2> where T1 : IModifier<T2>
    {
        [SerializeField]
        [TextArea(10,20)]
        private string description;
        [SerializeField]
        private Sprite thumbnail;
        [SerializeField]
        private int price;
        [SerializeField]
        private bool canBePurchased = true;
        [InlineEditor]
        [Space(5)]
        public T modifier;

        public string Description => description;
        public Sprite Thumbnail => thumbnail;
        public int Price => price;
        public bool CanBePurchased => canBePurchased;
    }
}