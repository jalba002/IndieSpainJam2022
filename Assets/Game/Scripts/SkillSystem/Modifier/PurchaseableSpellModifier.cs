using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(PurchaseableSpellModifier), menuName = "CosmosDefender/" + nameof(PurchaseableSpellModifier))]
    public class PurchaseableSpellModifier : BasePurchaseableModifier<PurchaseableSpellModifierData, BaseSpellModifier, ISpellModifier, SpellData>
    {
    }
}