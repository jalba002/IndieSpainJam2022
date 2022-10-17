using CosmosDefender.Shop;
using UnityEngine;

namespace CosmosDefender
{
    public interface IShopDisplayer : ILocalized
    {
        string Description { get; }
        Sprite Thumbnail { get; }
        int Price { get; }
        bool CanBePurchased { get; }
    }
}