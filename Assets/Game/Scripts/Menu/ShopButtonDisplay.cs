using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CosmosDefender
{
    public class ShopButtonDisplay : MonoBehaviour
    {
        [SerializeField]
        private Image image;
        [SerializeField]
        private TMP_Text description;
        [SerializeField]
        private TMP_Text price;

        public void ShowConfig<T, T1, T2, T3>(T config)
            where T : PurchaseableModifierData<T1, T2, T3>
            where T1 : BaseModifier<T2, T3>
            where T2 : IModifier<T3>
        {
            image.sprite = config.thumbnail;
            description.text = config.description;
            price.text = config.price.ToString();
        }
    }
}