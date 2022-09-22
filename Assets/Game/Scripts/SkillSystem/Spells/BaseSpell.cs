using System.Collections.Generic;
using System.Linq;
using CosmosDefender.Bullets;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CosmosDefender
{
    public abstract class BaseSpell : ScriptableObject, ISpell
    {
        [SerializeField]
        private SpellData baseData;

        [ShowInInspector, ReadOnly]
        protected SpellData currentData;
        [SerializeField]
        protected BaseBullet prefab;

        private PlayerAttributes playerAttributes;

        public SpellType spellType => baseData.SpellType;
        public SpellData spellData => currentData;

        public void SetPlayerAttributes(PlayerAttributes playerAttributes)
        {
            this.playerAttributes = playerAttributes;
        }

        public void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation) => Cast(spawnPoint, forward, rotation, playerAttributes.CombatData);
        protected abstract void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData);

        public void ApplyModifiers(IReadOnlyList<ISpellModifier> modifiers)
        {
            currentData = baseData;
            foreach (var modifier in modifiers.OrderBy(x => x.Priority))
            {
                if ((modifier.SpellType & spellType) == 0)
                    continue;

                modifier.Modify(ref currentData);
            }
        }
    }
}