using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CosmosDefender
{
    public abstract class BaseSpell : ScriptableObject, ISpell
    {
        [SerializeField]
        private SpellData baseData;

        protected SpellData currentData;
        [SerializeField]
        protected Bullet prefab;

        public abstract void Cast(Transform spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData);

        public void ApplyModifiers(IReadOnlyList<IModifier<SpellData>> modifiers)
        {
            currentData = baseData;
            foreach (var modifier in modifiers.OrderBy(x => x.Priority))
                modifier.Modify(ref currentData);
        }
    }
}