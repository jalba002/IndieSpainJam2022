using UnityEngine;

namespace CosmosDefender
{
    public interface IShopDisplayer
    {
        public string Description { get; }
        public Sprite Thumbnail { get; }
        public float Price { get; }
        public bool CanBePurchased { get; }
    }
}