using System;
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

        [SerializeField, Space] 
        private BaseSpell ultimateSpell;

        [ShowInInspector, ReadOnly]
        private AttributesData currentAttributes;

        private ObservableModifierList<BaseAttributeModifier, IAttributeModifier, AttributesData> attributeModifiers;
        private ObservableModifierList<BaseSpellModifier, ISpellModifier, SpellData> spellModifiers;

        [ShowInInspector]
        private List<BaseAttributeModifier> AttributeModifiers => attributeModifiers?.attributeModifiers;
        [ShowInInspector]
        private List<BaseSpellModifier> SpellModifiers => spellModifiers?.attributeModifiers;

        public event Action<IReadOnlyList<IBuffProvider>> onModifiersUpdated;

        public IReadOnlyOffensiveData CombatData => currentAttributes;
        public IReadOnlyDefensiveData DefensiveData => currentAttributes;
        public IReadOnlyMovementData SpeedData => currentAttributes;

        private bool isInitialized;

        public Action<CosmosSpell> OnSpellEmpowered;
        public Action<CosmosSpell, bool> OnSpellAdded;
        private List<IBuffProvider> cachedBuff = new List<IBuffProvider>();

        public void Initialize(bool forceInitialize = false)
        {
            if (isInitialized && !forceInitialize)
                return;

            attributeModifiers = new ObservableModifierList<BaseAttributeModifier, IAttributeModifier, AttributesData>(UpdateAttributes);
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
            ProvideModifiersBuffs();
        }

        private void UpdateSpells(IReadOnlyList<BaseSpellModifier> spellModifiers)
        {
            foreach (var spell in spells)
            {
                spell.ApplyModifiers(spellModifiers);
            }
            ultimateSpell.ApplyModifiers(spellModifiers);
            ProvideModifiersBuffs();
        }

        public IReadOnlyList<IBuffProvider> RequestBuffs()
        {
            cachedBuff.Clear();
            cachedBuff.AddRange(attributeModifiers.attributeModifiers);
            cachedBuff.AddRange(spellModifiers.attributeModifiers);
            return cachedBuff;
        }

        private void ProvideModifiersBuffs()
        {
            onModifiersUpdated?.Invoke(RequestBuffs());
        }

        public void EmpowerSpell(CosmosSpell spell, bool state)
        {
            // Does this work? Anyway, trigger event.
            spells.Find(x => x == spell).isSpellEmpowered = state;
            OnSpellEmpowered?.Invoke(spell);
            //spell.isSpellEmpowered = state;
        }

        public void EmpowerSpells(bool state)
        {
            foreach (var spell in spells)
            {
                spell.isSpellEmpowered = state;
            }
        }

        public AttributesData GetBaseAttributes() => baseAttributes;

        [Button]
        public void AddSpell(CosmosSpell spell, bool addSpell = true)
        {
            spells.Add(spell);
            spellModifiers.ForceUpdate();
            OnSpellAdded?.Invoke(spell, addSpell);
        }
        [Button]
        public bool HasSpellKey(SpellKeyType type) => spells.Count > (int)type;
        [Button]
        public BaseSpell GetSpell(SpellKeyType type) => spells[(int)type].GetSpell();
        public BaseSpell GetUltimate() => ultimateSpell;
        public List<CosmosSpell> GetAllSpells() => spells;
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