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
        
        [SerializeField] private string description;
        [SerializeField] private TMP_Text price;
        [SerializeField] private TMP_Text level;
        [SerializeField] private CanvasGroup descriptionPanel;
        
        [SerializeField] private Color validPurchase;
        [SerializeField] private Color invalidPurchase;

        private SpellDataPackage _spellDataPackage;
        private RectTransform panelTransform;

        private void Start()
        {
            descriptionPanel.Hide();
            panelTransform = descriptionPanel.GetComponent<RectTransform>();
        }

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
            //descriptionPanel.Show();
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            //descriptionPanel.Hide();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            // if (descriptionPanel.IsShown())
            // {
            //     var sizeDelta = panelTransform.rect;
            //     var realPos = Camera.main.ScreenToWorldPoint(eventData.position);
            //     //Vector3 offsetVector = new Vector3(sizeDelta.width * 0.5f, -sizeDelta.height * 0.5f, 0f);
            //     //realPos += offsetVector;
            //     //Debug.Log(realPos + "\n" + offsetVector);
            //     
            //     realPos.z = 150f;
            //
            //     panelTransform. = realPos;
            // }
            
            // Instead of moving the descPanel, just call the static class to move. :D
        }
    }
}