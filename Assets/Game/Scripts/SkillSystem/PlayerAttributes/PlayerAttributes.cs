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
        private BaseSpell[] spells;

        //[ShowInInspector, ReadOnly]
        private AttributesData currentAttributes;

        private Dictionary<SpellType, BaseSpell> indexedSpells;
        private ObservableModifierList<BaseAttributeModifier, ISpellModifier<AttributesData>, AttributesData> attributeModifiers;
        private ObservableModifierList<BaseSpellModifier, ISpellModifier, SpellData> spellModifiers;

        public IReadOnlyOffensiveData CombatData => currentAttributes;
        public IReadOnlyDefensiveData DefensiveData => currentAttributes;
        public IReadOnlyMovementData SpeedData => currentAttributes;

        public void Initialize()
        {
            indexedSpells = spells.ToDictionary(x => x.spellType);
            attributeModifiers = new ObservableModifierList<BaseAttributeModifier, ISpellModifier<AttributesData>, AttributesData>(UpdateAttributes);
            spellModifiers = new ObservableModifierList<BaseSpellModifier, ISpellModifier, SpellData>(UpdateSpells);

            RemoveAllAttributeModifiers();
            RemoveAllSpellModifiers();
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
                spell.ApplyModifiers(spellModifiers);
        }

        [Button]
        public BaseSpell GetSpell(SpellType type) => indexedSpells[type];

        public void AddAttributeModifier(BaseAttributeModifier modifier) => attributeModifiers.AddModifier(modifier);
        public void AddAttributeModifiers(List<BaseAttributeModifier> modifiers) => attributeModifiers.AddModifiers(modifiers);
        public void RemoveAttributeModifier(BaseAttributeModifier modifier) => attributeModifiers.RemoveModifier(modifier);
        public void RemoveAllAttributeModifiers() => attributeModifiers.RemoveAllModifiers();

        [Button]
        public void AddSpellModifier(BaseSpellModifier modifier) => spellModifiers.AddModifier(modifier);
        public void AddSpellModifiers(List<BaseSpellModifier> modifier) => spellModifiers.AddModifiers(modifier);
        public void RemoveSpellModifier(BaseSpellModifier modifier) => spellModifiers.RemoveModifier(modifier);
        public void RemoveAllSpellModifiers() => spellModifiers.RemoveAllModifiers();
    }
}