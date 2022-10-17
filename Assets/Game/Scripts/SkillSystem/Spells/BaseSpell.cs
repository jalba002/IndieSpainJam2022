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

        public SpellType spellType => baseData.SpellType;
        public CastType castType => baseData.CastType;
        
        public SpellData spellData => currentData;

        public SpellData BaseData => baseData;

        //public void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData) => Cast(spawnPoint, forward, rotation, combatData, null);
        //public void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData, ISpellCaster caster) => Cast(spawnPoint, forward, rotation, combatData, caster);
        public abstract void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData, ISpellCaster caster);

        public abstract void StopCast();

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

        public void UpdateCurrentData()
        {
            currentData = baseData;
        }
    }
}