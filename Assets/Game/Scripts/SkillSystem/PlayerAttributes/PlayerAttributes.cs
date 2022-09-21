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
        [SerializeField]
        private BaseSpell[] spells;

        [ShowInInspector, ReadOnly]
        private AttributesData currentAttributes;

        private ObservableModifierList<BaseModifier<AttributesData>, AttributesData> attributeModifiers;
        private ObservableModifierList<BaseModifier<SpellData>, SpellData> spellModifiers;

        public IReadOnlyOffensiveData CombatData => currentAttributes;
        public IReadOnlyDefensiveData DefensiveData => currentAttributes;
        public IReadOnlyMovementData SpeedData => currentAttributes;

        public void Initialize()
        {
            attributeModifiers = new ObservableModifierList<BaseModifier<AttributesData>, AttributesData>(UpdateAttributes);
            spellModifiers = new ObservableModifierList<BaseModifier<SpellData>, SpellData>(UpdateSpells);
        }

        private void UpdateAttributes(IReadOnlyList<BaseModifier<AttributesData>> attributeModifiers)
        {
            currentAttributes = baseAttributes;
            foreach (var modifier in attributeModifiers.OrderBy(x => x.Priority))
                modifier.Modify(ref currentAttributes);
        }

        private void UpdateSpells(IReadOnlyList<BaseModifier<SpellData>> spellModifiers)
        {
            foreach (var spell in spells)
                spell.ApplyModifiers(spellModifiers);
        }

        public void AddAttributeModifier(BaseModifier<AttributesData> modifier) => attributeModifiers.AddModifier(modifier);
        public void AddAttributeModifiers(List<BaseModifier<AttributesData>> modifiers) => attributeModifiers.AddModifiers(modifiers);
        public void RemoveAttributeModifier(BaseModifier<AttributesData> modifier) => attributeModifiers.RemoveModifier(modifier);
        public void RemoveAllAttributeModifiers() => attributeModifiers.RemoveAllModifiers();

        public void AddSpellModifier(BaseModifier<SpellData> modifier) => spellModifiers.AddModifier(modifier);
        public void AddSpellModifiers(List<BaseModifier<SpellData>> modifier) => spellModifiers.AddModifiers(modifier);
        public void RemoveSpellModifier(BaseModifier<SpellData> modifier) => spellModifiers.RemoveModifier(modifier);
        public void RemoveAllSpellModifiers() => spellModifiers.RemoveAllModifiers();
    }
}