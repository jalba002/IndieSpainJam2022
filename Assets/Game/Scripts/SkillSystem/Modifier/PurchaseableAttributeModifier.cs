using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(PurchaseableAttributeModifierData), menuName = "CosmosDefender/" + nameof(PurchaseableAttributeModifierData))]
    public class PurchaseableAttributeModifier : BasePurchaseableAttributeModifier<PurchaseableAttributeModifierData, BaseAttributeModifier, ISpellModifier<AttributesData>, AttributesData>
    {
    }
}