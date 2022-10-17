using CosmosDefender.Shop;
using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(PurchaseableAttributeModifier), menuName = "CosmosDefender/" + nameof(PurchaseableAttributeModifier))]
    public class PurchaseableAttributeModifier : BasePurchaseableModifier<PurchaseableAttributeModifierData, BaseAttributeModifier, IAttributeModifier, AttributesData>
    {
    }
}