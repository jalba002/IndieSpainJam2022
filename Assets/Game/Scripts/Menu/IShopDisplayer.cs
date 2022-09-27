using UnityEngine;

namespace CosmosDefender
{
    public interface IShopDisplayer
    {
        string Description { get; }
        Sprite Thumbnail { get; }
        int Price { get; }
        bool CanBePurchased { get; }
    }
}