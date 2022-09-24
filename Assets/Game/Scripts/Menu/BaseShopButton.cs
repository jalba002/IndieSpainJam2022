using UnityEngine;

namespace CosmosDefender
{
    public class BaseShopButton<TPurchase, T, T1, T2, T3> : MonoBehaviour
        where TPurchase : BasePurchaseableModifier<T, T1, T2, T3>
        where T : PurchaseableModifierData<T1, T2, T3>
        where T1 : BaseModifier<T2, T3>
        where T2 : IModifier<T3>
    {
        private ShopButtonDisplay buttonDisplay;

        private TPurchase config;
        private T currentModifier;

        private void Awake()
        {
            buttonDisplay = GetComponent<ShopButtonDisplay>();
        }

        public void Initialize(TPurchase config)
        {
            this.config = config;
        }

        public void Show()
        {
            currentModifier = config.GetCurrentPurchaseable();
            buttonDisplay.ShowConfig<T, T1, T2, T3>(currentModifier);
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