using System.Collections.Generic;
using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(PurchasedAttributes), menuName = "CosmosDefender/" + nameof(PurchasedAttributes))]
    public class PurchasedAttributes : ScriptableObject
    {
        [SerializeField]
        private List<PurchaseableAttributeModifier> purchasedAttributes;

        [SerializeField]
        private List<PurchaseableAttributeModifier> allAttributesShop;
    }
}