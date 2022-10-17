using UnityEngine;

namespace CosmosDefender
{
    public abstract class BaseSpellModifier : BaseModifier<ISpellModifier, SpellData>, ISpellModifier
    {
        [Header("Spell Data")]
        [SerializeField]
        private SpellType spellType;

        public SpellType SpellType => spellType;
    }
}