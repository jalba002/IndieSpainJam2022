using UnityEngine;

namespace CosmosDefender
{
    public class PurchaseableModifierData<T, T1, T2> : ScriptableObject, IShopDisplayer where T : BaseModifier<T1, T2> where T1 : IModifier<T2>
    {
        [SerializeField]
        private string description;
        [SerializeField]
        private Sprite thumbnail;
        [SerializeField]
        private float price;
        [SerializeField]
        private bool canBePurchase = true;
        public T modifier;

        public string Description => description;
        public Sprite Thumbnail => thumbnail;
        public float Price => price;
        public bool CanBePurchased => canBePurchase;
    }
}