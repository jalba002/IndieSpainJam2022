using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(PurchaseableSpellModifierData), menuName = "CosmosDefender/" + nameof(PurchaseableSpellModifierData))]
    public class PurchaseableSpellModifierData : PurchaseableModifierData<BaseSpellModifier, ISpellModifier, SpellData> { }
}