using System;

namespace CosmosDefender
{
    public class PurchaseableModifierData<T, T1, T2> where T : BaseModifier<T1, T2> where T1 : ISpellModifier<T2>
    {
        public float price;
        public T modifier;
    }

    [Serializable]
    public class PurchaseableAttributeModifierData : PurchaseableModifierData<BaseAttributeModifier, ISpellModifier<AttributesData>, AttributesData> { }
    [Serializable]
    public class PurchaseableSpellModifierData : PurchaseableModifierData<BaseSpellModifier, ISpellModifier, SpellData> { }
}