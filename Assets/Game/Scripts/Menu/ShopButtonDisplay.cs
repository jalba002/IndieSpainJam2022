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
        private TMP_Text level;
        [SerializeField]
        private CanvasGroup descriptionPanel;

        private void Awake()
        {
            descriptionPanel.Hide();
        }

        public void ShowConfig(IShopDisplayer displayer, int level)
        {
            image.sprite = displayer.Thumbnail;
            description.text = displayer.Description;
            price.text = !displayer.CanBePurchased ? "None" : displayer.Price.ToString();
            this.level.text = level.ToString();
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