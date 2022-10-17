using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CosmosDefender.Shop
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TextDisplayer : MonoSingleton<TextDisplayer>
    {
        [SerializeField] private TMP_Text textString;
        [SerializeField] private CanvasGroup selfGroup;
        [SerializeField] private Vector2 offsetVector = Vector2.one;

        protected override bool dontDestroyOnLoad => false;

        private string textReference;

        [SerializeField] private LocalizeStringEvent textLocalized;

        public void DisplayTranslatedText(string tableRef, string entryRef, string initial, string finalValue)
        {
            textLocalized.StringReference = new LocalizedString
            {
                {"initialValue", new StringVariable() {Value = initial}},
                {"finalValue", new StringVariable() {Value = finalValue}}
            };
            
            textLocalized.SetTable(tableRef);
            textLocalized.SetEntry(entryRef);
        }

        public void Show()
        {
            selfGroup.Show();
        }

        public void Hide()
        {
            selfGroup.Hide();
        }

        public void Move(PointerEventData eventData)
        {
            var offset = ((RectTransform) transform).sizeDelta * offsetVector;
            var vector = Camera.main.ScreenToWorldPoint(eventData.position + offset);
            vector.z = 150f;
            transform.position = vector;
        }
    }
}