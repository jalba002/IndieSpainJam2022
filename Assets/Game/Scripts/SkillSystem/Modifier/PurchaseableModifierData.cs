using System;
using System.Globalization;
using CosmosDefender.Shop;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

namespace CosmosDefender
{
    public class PurchaseableModifierData<T, T1, T2> : ScriptableObject, IShopDisplayer, IShopDescription<T2> where T : BaseModifier<T1, T2> where T1 : IModifier<T2>
    {
        [Header("Descriptions & Language Settings")]
        [SerializeField] private string description;
        [SerializeField] private string translationTable;
        [SerializeField] private string translationCode;

        [Header("Data")]
        [SerializeField] private int price;
        [SerializeField] private bool canBePurchased = true;
        
        [Header("Beauty")]
        [SerializeField] private Sprite thumbnail;
        
        [Header("Modifier")]
        [InlineEditor] [Space(10)] public T modifier;

        public string Description => description;
        public Sprite Thumbnail => thumbnail;

        public int Price
        {
            get => price;
            set => price = value;
        }

        public bool CanBePurchased => canBePurchased;
        public string TableReference => translationTable;
        public string EntryReference => translationCode;
       
        public string InitialValue(T2 data) 
        {
            return ConvertToStringFormatted(modifier.GetInitialValue(data));
        }

        private string ConvertToStringFormatted(float value)
        {
            var returnString = "A";
            
            returnString = value.ToString(Math.Abs(value % 1) <= 0.001f ? "F0" : "F2");

            return returnString;
        }

        public string FinalValue(T2 data)
        {
            // If decimal, then F2, if not. Int.
            return ConvertToStringFormatted(modifier.GetFinalValue(data));
        }
    }
}