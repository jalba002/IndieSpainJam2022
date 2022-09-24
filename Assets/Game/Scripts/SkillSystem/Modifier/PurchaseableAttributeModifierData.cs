using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(PurchaseableAttributeModifierData), menuName = "CosmosDefender/" + nameof(PurchaseableAttributeModifierData))]
    public class PurchaseableAttributeModifierData : PurchaseableModifierData<BaseAttributeModifier, IModifier<AttributesData>, AttributesData> { }
}