using TMPro;
using UnityEngine;

namespace CosmosDefender.Shop
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TextDisplayer : MonoSingleton<TextDisplayer>
    {
        [SerializeField] private TMP_Text textString;
        [SerializeField] private CanvasGroup selfGroup;

        protected override bool dontDestroyOnLoad => false;

        public void DisplayText(string text)
        {
            textString.text = text;
        }

        public void Show()
        {
            selfGroup.Show();
        }

        public void Hide()
        {
            selfGroup.Hide();
        }
    }
}