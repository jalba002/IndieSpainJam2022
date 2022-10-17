using CosmosDefender.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CosmosDefender
{
    public class ShopButtonDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text price;
        [SerializeField] private TMP_Text level;
        [Header("Colors")]
        [SerializeField] private Color validPurchase;
        [SerializeField] private Color invalidPurchase;

        private RectTransform panelTransform;

        private IShopDisplayer lastDisplayer;

        private string initialValue;
        private string finalValue;

        public void ShowConfig(IShopDisplayer displayer, string entry, string final, int level, int moneyAmount,bool displayText = false)
        {
            lastDisplayer = displayer;
            image.sprite = displayer.Thumbnail;

            initialValue = entry;
            finalValue = final;

            if (displayText)
            {
                // Now, where the fuck do we get the data from.
                TextDisplayer.Instance.DisplayTranslatedText(displayer.TableReference, displayer.EntryReference, entry, final);
                //TextDisplayer.Instance.DisplayText(description);
            }

            price.color = moneyAmount >= displayer.Price ? validPurchase : invalidPurchase;
            price.text = !displayer.CanBePurchased ? "max" : displayer.Price.ToString();
            this.level.text = !displayer.CanBePurchased ? "" : level.ToString();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            TextDisplayer.Instance.Show();
            //TextDisplayer.Instance.DisplayText(description);
            TextDisplayer.Instance.DisplayTranslatedText(lastDisplayer.TableReference, lastDisplayer.EntryReference, initialValue, finalValue);
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