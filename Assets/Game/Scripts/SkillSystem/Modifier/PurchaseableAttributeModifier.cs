using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(PurchaseableAttributeModifierData), menuName = "CosmosDefender/" + nameof(PurchaseableAttributeModifierData))]
    public class PurchaseableAttributeModifier : BasePurchaseableModifier<PurchaseableAttributeModifierData, BaseAttributeModifier, IModifier<AttributesData>, AttributesData>
    {
    }
}