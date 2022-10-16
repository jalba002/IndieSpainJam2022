using System;
using CosmosDefender.Shop;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CosmosDefender
{
    public class ShopButtonDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
    {
        [SerializeField] private Image image;
        public string description;
        [SerializeField] private TMP_Text price;
        [SerializeField] private TMP_Text level;
        [Header("Colors")]
        [SerializeField] private Color validPurchase;
        [SerializeField] private Color invalidPurchase;

        private RectTransform panelTransform;

        public void ShowConfig(IShopDisplayer displayer, int level, int moneyAmount)
        {
            image.sprite = displayer.Thumbnail;
            description = displayer.Description;
            // The new description should be as following:
            // Send the translation text and table to the string translator.
            // Send the data values to the string variables table.
            // Being the initial and the final values of them.
            // displayer.

            price.color = moneyAmount >= displayer.Price ? validPurchase : invalidPurchase;
            price.text = !displayer.CanBePurchased ? "max" : displayer.Price.ToString();
            this.level.text = !displayer.CanBePurchased ? "" : level.ToString();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            TextDisplayer.Instance.Show();
            TextDisplayer.Instance.DisplayText(description);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            TextDisplayer.Instance.Hide();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            TextDisplayer.Instance.Move(eventData);
        }
    }
}