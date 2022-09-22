using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CosmosDefender
{
    public class BaseShopButton<TPurchase, T, T1, T2, T3> : MonoBehaviour
        where TPurchase : BasePurchaseableModifier<T, T1, T2, T3>
        where T : PurchaseableModifierData<T1, T2, T3>
        where T1 : BaseModifier<T2, T3>
        where T2 : IModifier<T3>
    {
        [SerializeField]
        private TPurchase config;
        private Image image;
        private TMP_Text description;

        private T currentModifier;

        public void Initialize(TPurchase config)
        {
            this.config = config;
        }

        public void Show()
        {
            currentModifier = config.GetCurrentPurchaseable();
            image.sprite = currentModifier.thumbnail;
            description.text = currentModifier.description;
        }

        public void OnClick()
        {
            //TODO: Check money!
            if (/*have enough money*/ true)
            {
                config.Purchase(currentModifier);
                Show();
            }
        }
    }
}