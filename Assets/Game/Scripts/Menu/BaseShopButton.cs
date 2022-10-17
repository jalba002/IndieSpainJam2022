using UnityEngine;

namespace CosmosDefender
{
    public class BaseShopButton<TPurchase, T, T1, T2, T3> : MonoBehaviour, IShopButton
        where TPurchase : BasePurchaseableModifier<T, T1, T2, T3>
        where T : PurchaseableModifierData<T1, T2, T3>
        where T1 : BaseModifier<T2, T3>
        where T2 : IModifier<T3>
    {
        private ShopButtonDisplay buttonDisplay;

        private TPurchase config;
        private T currentModifier;
        private EconomyConfig economyConfig;
        private T3 relevantData;

        private void Awake()
        {
            buttonDisplay = GetComponent<ShopButtonDisplay>();
        }

        public void Initialize(TPurchase config, EconomyConfig economyConfig, T3 relevantData)
        {
            this.config = config;
            this.economyConfig = economyConfig;
            this.relevantData = relevantData;
        }

        public void Show(bool showText = false)
        {
            currentModifier = config.GetCurrentPurchaseable();

            // If the previous modifier is the -1, then just get the current one and it will gather the correct value.
            string previousValue = "0";
            bool initial = config.GetCurrentPurchaseIndex() > 0;
            var previousModifier = initial ? config.GetCurrentPurchasedItem() : config.GetCurrentPurchaseable();

            if (previousModifier.CanBePurchased)
            {
                previousValue = initial ? previousModifier.FinalValue(relevantData) : previousModifier.InitialValue(relevantData);
            }
            
            var finalValue = currentModifier.CanBePurchased ? currentModifier.FinalValue(relevantData) : "0";

            var currentIndex = config.GetCurrentPurchaseIndex();
            buttonDisplay.ShowConfig(currentModifier, previousValue, finalValue, currentIndex + 1,
                economyConfig.GetMoney(), showText);
        }

        public void OnClick()
        {
            if (currentModifier.CanBePurchased && economyConfig.GetMoney() >= currentModifier.Price)
            {
                config.Purchase(currentModifier);
                economyConfig.SubstractMoney(currentModifier.Price);
                Show(true);
            }
        }

        protected virtual T3 GetData() => relevantData;
    }
}