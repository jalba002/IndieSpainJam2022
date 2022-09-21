using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(PurchaseableSpellModifier), menuName = "CosmosDefender/" + nameof(PurchaseableSpellModifier))]
    public class PurchaseableSpellModifier : BasePurchaseableAttributeModifier<PurchaseableSpellModifierData, BaseSpellModifier, ISpellModifier, SpellData>
    {
    }
}