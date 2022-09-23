using CosmosDefender;
using Sirenix.OdinInspector;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField]
    private ShopModifiers shopModifiers;
    [SerializeField]
    private AttributeShopButton attributePrefab;
    [SerializeField]
    private SpellShopButton spellPrefab;

    private void Awake()
    {
        InitializeShop();
    }

    private void InitializeShop()
    {
        foreach (var item in shopModifiers.AttributesModifierShop)
        {
            var button = Instantiate(attributePrefab, transform);
            button.Initialize(item);
            button.Show();
        }

        foreach (var item in shopModifiers.SpellModifierShop)
        {
            var button = Instantiate(spellPrefab, transform);
            button.Initialize(item);
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
