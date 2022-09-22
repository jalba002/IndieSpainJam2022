using System;
using UnityEngine;

namespace CosmosDefender
{
    public class PurchaseableModifierData<T, T1, T2> where T : BaseModifier<T1, T2> where T1 : IModifier<T2>
    {
        public string description;
        public Sprite thumbnail;
        public float price;
        public T modifier;
    }

    [Serializable]
    public class PurchaseableAttributeModifierData : PurchaseableModifierData<BaseAttributeModifier, IModifier<AttributesData>, AttributesData> { }
    [Serializable]
    public class PurchaseableSpellModifierData : PurchaseableModifierData<BaseSpellModifier, ISpellModifier, SpellData> { }
}