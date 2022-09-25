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
        private EconomyConfig economyConfig;

        private void Awake()
        {
            buttonDisplay = GetComponent<ShopButtonDisplay>();
        }

        public void Initialize(TPurchase config, EconomyConfig economyConfig)
        {
            this.config = config;
            this.economyConfig = economyConfig;
        }

        public void Show()
        {
            currentModifier = config.GetCurrentPurchaseable();
            var currentIndex = config.GetCurrentPurchaseIndex();
            buttonDisplay.ShowConfig(currentModifier, currentIndex + 1);
        }

        public void OnClick()
        {
            if (currentModifier.CanBePurchased && economyConfig.GetMoney() > currentModifier.Price)
            {
                config.Purchase(currentModifier);
                economyConfig.SubstractMoney(currentModifier.Price);
                Show();
            }
        }
    }
}