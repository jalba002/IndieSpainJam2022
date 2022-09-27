using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(ShopModifiers), menuName = "CosmosDefender/" + nameof(ShopModifiers))]
    public class ShopModifiers : ScriptableObject
    {
        private const string AttributeKey = nameof(AttributeKey);
        private const string SpellKey = nameof(SpellKey);

        [SerializeField, InlineEditor]
        private List<PurchaseableAttributeModifier> attributesModifierShop;
        [SerializeField, InlineEditor]
        private List<PurchaseableSpellModifier> spellModifierShop;

        [SerializeField]
        private PurchaseableAttributeModifierData lastAttributePurchase;
        [SerializeField]
        private PurchaseableSpellModifierData lastSpellPurchase;

        public bool addLastPurchases;

        public IReadOnlyList<PurchaseableAttributeModifier> AttributesModifierShop => attributesModifierShop;
        public IReadOnlyList<PurchaseableSpellModifier> SpellModifierShop => spellModifierShop;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            Load();
            if (addLastPurchases)
            {
                attributesModifierShop.ForEach(x => x.AddUniqueModifierToPurchase(lastAttributePurchase));
                spellModifierShop.ForEach(x => x.AddUniqueModifierToPurchase(lastSpellPurchase));
            }
        }

        public void Save()
        {
            SaveModifierData(attributesModifierShop.Select(x => x.ShopData), AttributeKey);
            SaveModifierData(spellModifierShop.Select(x => x.ShopData), SpellKey);
        }

        private void SaveModifierData(IEnumerable<SerializableShopModifier> purchaseableData, string key)
        {
            var serializedModifier = new SerializedShop(purchaseableData.ToList());
            PlayerPrefs.SetString(key, JsonUtility.ToJson(serializedModifier));
            PlayerPrefs.Save();
        }

        public void Load()
        {
            attributesModifierShop.ForEach(x => x.ShopData = new SerializableShopModifier());
            var serializedAttributes = JsonUtility.FromJson<SerializedShop>(PlayerPrefs.GetString(AttributeKey, "{}"));
            for (int i = 0; i < serializedAttributes.modifiersPurchased.Count; i++)
            {
                attributesModifierShop[i].ShopData = serializedAttributes.modifiersPurchased[i];
            }
            
            spellModifierShop.ForEach(x => x.ShopData = new SerializableShopModifier());
            var serializedSpells = JsonUtility.FromJson<SerializedShop>(PlayerPrefs.GetString(SpellKey, "{}"));
            for (int i = 0; i < serializedSpells.modifiersPurchased.Count; i++)
            {
                spellModifierShop[i].ShopData = serializedSpells.modifiersPurchased[i];
            }
        }

        public List<BaseAttributeModifier> GetAttributeModifiers()
        {
            List<BaseAttributeModifier> attributes = new List<BaseAttributeModifier>();
            attributesModifierShop.ForEach(x => attributes.AddRange(x.GetPurchasedSpells()));
            return attributes;
        }

        public List<BaseSpellModifier> GetSpellModifiers()
        {
            List<BaseSpellModifier> attributes = new List<BaseSpellModifier>();
            spellModifierShop.ForEach(x => attributes.AddRange(x.GetPurchasedSpells()));
            return attributes;
        }

        [Button]
        public void ResetShop()
        {
            PlayerPrefs.DeleteKey(AttributeKey);
            PlayerPrefs.DeleteKey(SpellKey);
            PlayerPrefs.Save();
        }
    }

    [Serializable]
    public class SerializedShop
    {
        public List<SerializableShopModifier> modifiersPurchased;

        public SerializedShop(List<SerializableShopModifier> modifiersPurchased)
        {
            this.modifiersPurchased = modifiersPurchased;
        }
    }
}