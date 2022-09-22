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

        public IReadOnlyList<PurchaseableAttributeModifier> AttributesModifierShop => attributesModifierShop;
        public IReadOnlyList<PurchaseableSpellModifier> SpellModifierShop => spellModifierShop;

        public void Initialize()
        {
            Load();
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
        }

        public void Load()
        {
            var serializedAttributes = JsonUtility.FromJson<SerializedShop>(PlayerPrefs.GetString(AttributeKey, "{}"));
            for (int i = 0; i < serializedAttributes.modifiersPurchased.Count; i++)
                attributesModifierShop[i].ShopData = serializedAttributes.modifiersPurchased[i];

            var serializedSpells = JsonUtility.FromJson<SerializedShop>(PlayerPrefs.GetString(SpellKey, "{}"));
            for (int i = 0; i < serializedSpells.modifiersPurchased.Count; i++)
                spellModifierShop[i].ShopData = serializedSpells.modifiersPurchased[i];
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