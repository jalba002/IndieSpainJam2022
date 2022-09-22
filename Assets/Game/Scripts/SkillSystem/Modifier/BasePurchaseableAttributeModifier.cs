using System.Collections.Generic;
using UnityEngine;

namespace CosmosDefender
{
    public abstract class BasePurchaseableAttributeModifier<T, T1, T2, T3> : ScriptableObject
        where T : PurchaseableModifierData<T1, T2, T3>
        where T1 : BaseModifier<T2, T3>
        where T2 : ISpellModifier<T3>
    {
        [SerializeField]
        private List<T> modifers;

        private float purchasedCount;
    }
}