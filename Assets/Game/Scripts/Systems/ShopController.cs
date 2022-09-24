using CosmosDefender;
using Sirenix.OdinInspector;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField]
    private ShopModifiers shopModifiers;
    [SerializeField]
    private EconomyConfig economyConfig;
    [SerializeField]
    private AttributeShopButton attributePrefab;
    [SerializeField]
    private SpellShopButton spellPrefab;
    [SerializeField]
    private RectTransform attributesGrid;
    [SerializeField]
    private RectTransform spellsGrid;

    private void Awake()
    {
        InitializeShop();
    }

    private void InitializeShop()
    {
        foreach (var item in shopModifiers.AttributesModifierShop)
        {
            var button = Instantiate(attributePrefab, attributesGrid);
            button.Initialize(item, economyConfig);
            button.Show();
        }

        foreach (var item in shopModifiers.SpellModifierShop)
        {
            var button = Instantiate(spellPrefab, spellsGrid);
            button.Initialize(item, economyConfig);
            button.Show();
        }
    }

    public void SaveShop()
    {
        shopModifiers.Save();
    }

    [Button]
    private void ResetShop()
    {
        shopModifiers.ResetShop();
    }
}
