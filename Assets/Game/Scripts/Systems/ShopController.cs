using System;
using System.Collections.Generic;
using CosmosDefender;
using Sirenix.OdinInspector;
using TMPro;
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
    [SerializeField]
    private TMP_Text currentMoney;

    private List<IShopButton> shopButtons = new List<IShopButton>();

    private void Awake()
    {
        InitializeShop();
        UpdateMoney(economyConfig.GetMoney());
        economyConfig.OnMoneyUpdated += UpdateMoney;
    }

    private void OnDestroy()
    {
        economyConfig.OnMoneyUpdated -= UpdateMoney;
    }

    private void InitializeShop()
    {
        foreach (var item in shopModifiers.AttributesModifierShop)
        {
            var button = Instantiate(attributePrefab, attributesGrid);
            button.Initialize(item, economyConfig);
            shopButtons.Add(button);
        }

        foreach (var item in shopModifiers.SpellModifierShop)
        {
            var button = Instantiate(spellPrefab, spellsGrid);
            button.Initialize(item, economyConfig);
            shopButtons.Add(button);
        }

        RefreshShop();
    }

    public void RefreshShop()
    {
        foreach (var item in shopButtons)
            item.Show();
    }

    private void UpdateMoney(int money)
    {
        currentMoney.text = money.ToString();
        RefreshShop();
    }

    public void SaveShop()
    {
        shopModifiers.Save();
    }

    private void OnDisable()
    {
        SaveShop();
    }

    [Button]
    private void ResetShop()
    {
        shopModifiers.ResetShop();
    }
}
