using System.Collections.Generic;
using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(ShopAttributesModifier), menuName = "CosmosDefender/" + nameof(ShopAttributesModifier))]
    public class ShopAttributesModifier : ScriptableObject
    {
        [SerializeField]
        private List<PurchaseableAttributeModifier> purchasedAttributes;

        [SerializeField]
        private List<PurchaseableAttributeModifier> allAttributesShop;
    }
}