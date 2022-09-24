using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(PurchaseableAttributeModifier), menuName = "CosmosDefender/" + nameof(PurchaseableAttributeModifier))]
    public class PurchaseableAttributeModifier : BasePurchaseableModifier<PurchaseableAttributeModifierData, BaseAttributeModifier, IModifier<AttributesData>, AttributesData>
    {
    }
}