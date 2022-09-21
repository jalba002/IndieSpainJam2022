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

            RemoveAllAttributeModifiers();
            RemoveAllSpellModifiers();
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

        [Button]
        public void AddAttributeModifier(BaseAttributeModifier modifier) => attributeModifiers.AddModifier(modifier);
        [Button]
        public void AddAttributeModifiers(List<BaseAttributeModifier> modifiers) => attributeModifiers.AddModifiers(modifiers);
        [Button]
        public void RemoveAttributeModifier(BaseAttributeModifier modifier) => attributeModifiers.RemoveModifier(modifier);
        [Button]
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