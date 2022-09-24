using UnityEngine;

namespace CosmosDefender
{
    public interface IShopDisplayer
    {
        public string Description { get; }
        public Sprite Thumbnail { get; }
        public int Price { get; }
        public bool CanBePurchased { get; }
    }
}