using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CosmosDefender
{
    public class ShopButtonDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Image image;
        [SerializeField]
        private TMP_Text description;
        [SerializeField]
        private TMP_Text price;
        [SerializeField]
        private CanvasGroup descriptionPanel;

        private void Awake()
        {
            descriptionPanel.Hide();
        }

        public void ShowConfig(IShopDisplayer displayer)
        {
            price.gameObject.SetActive(displayer.CanBePurchased);
            image.sprite = displayer.Thumbnail;
            description.text = displayer.Description;
            price.text = displayer.Price.ToString();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            descriptionPanel.Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            descriptionPanel.Hide();
        }
    }
}