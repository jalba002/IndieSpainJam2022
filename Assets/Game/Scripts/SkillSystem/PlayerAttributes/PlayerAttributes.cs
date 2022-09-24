using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(PlayerAttributes), menuName = "CosmosDefender/" + nameof(PlayerAttributes))]
    public class PlayerAttributes : ScriptableObject
    {
        [SerializeField]
        private AttributesData baseAttributes;
        [SerializeField, Space]
        private List<CosmosSpell> spells;

        [ShowInInspector, ReadOnly]
        private AttributesData currentAttributes;

        private ObservableModifierList<BaseAttributeModifier, IModifier<AttributesData>, AttributesData> attributeModifiers;
        private ObservableModifierList<BaseSpellModifier, ISpellModifier, SpellData> spellModifiers;

        [ShowInInspector]
        private List<BaseAttributeModifier> AttributeModifiers => attributeModifiers?.attributeModifiers;
        [ShowInInspector]
        private List<BaseSpellModifier> SpellModifiers => spellModifiers?.attributeModifiers;

        public IReadOnlyOffensiveData CombatData => currentAttributes;
        public IReadOnlyDefensiveData DefensiveData => currentAttributes;
        public IReadOnlyMovementData SpeedData => currentAttributes;

        private bool isInitialized;

        public void Initialize(bool forceInitialize = false)
        {
            if (isInitialized && !forceInitialize)
                return;

            attributeModifiers = new ObservableModifierList<BaseAttributeModifier, IModifier<AttributesData>, AttributesData>(UpdateAttributes);
            spellModifiers = new ObservableModifierList<BaseSpellModifier, ISpellModifier, SpellData>(UpdateSpells);

            attributeModifiers.ForceUpdate();
            spellModifiers.ForceUpdate();

            isInitialized = true;
        }

        private void UpdateAttributes(IReadOnlyList<BaseAttributeModifier> attributeModifiers)
        {
            currentAttributes = baseAttributes;
            foreach (var modifier in attributeModifiers.OrderBy(x => x.Priority))
                modifier.Modify(ref currentAttributes);
        }

        private void UpdateSpells(IReadOnlyList<BaseSpellModifier> spellModifiers)
        {
            foreach (var spell in spells)
            {
                spell.ApplyModifiers(spellModifiers);
            }
        }

        [Button]
        public void AddSpell(CosmosSpell spell)
        {
            spells.Add(spell);
            spellModifiers.ForceUpdate();
        }
        [Button]
        public bool HasSpellKey(SpellKeyType type) => spells.Count > (int)type;
        [Button]
        public BaseSpell GetSpell(SpellKeyType type) => spells[(int)type].GetSpell();
        public void RemoveAllSpells() => spells.Clear();

        public void AddAttributeModifier(BaseAttributeModifier modifier) => attributeModifiers.AddModifier(modifier);
        public void AddAttributeModifiers(List<BaseAttributeModifier> modifiers) => attributeModifiers.AddModifiers(modifiers);
        public void RemoveAttributeModifier(BaseAttributeModifier modifier) => attributeModifiers.RemoveModifier(modifier);
        public void RemoveAllAttributeModifiers() => attributeModifiers.RemoveAllModifiers();

        [Button]
        public void AddSpellModifier(BaseSpellModifier modifier) => spellModifiers.AddModifier(modifier);
        [Button]
        public void AddSpellModifiers(List<BaseSpellModifier> modifier) => spellModifiers.AddModifiers(modifier);
        [Button]
        public void RemoveSpellModifier(BaseSpellModifier modifier) => spellModifiers.RemoveModifier(modifier);
        [Button]
        public void RemoveAllSpellModifiers() => spellModifiers.RemoveAllModifiers();
    }
}