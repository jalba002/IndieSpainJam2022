using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CosmosDefender.Shop
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TextDisplayer : MonoSingleton<TextDisplayer>
    {
        [SerializeField] private TMP_Text textString;
        [SerializeField] private CanvasGroup selfGroup;
        [SerializeField] private Vector2 offsetVector = Vector2.one;

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

        public void Move(PointerEventData eventData)
        {
            var offset = ((RectTransform)transform).sizeDelta * offsetVector;
            var vector = Camera.main.ScreenToWorldPoint(eventData.position + offset);
            vector.z = 150f;
            transform.position = vector;
        }
        
        public void Move(Vector2 pos)
        {
            
        }
    }
}