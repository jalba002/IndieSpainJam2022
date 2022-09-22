using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using CosmosDefender.Bullets;

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

        public SpellType spellType => baseData.SpellType;

        public abstract void Cast(Transform spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData);

        [Button("Update Current Data")]
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