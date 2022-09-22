using System.Collections.Generic;
using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(CosmosSpell), menuName = "CosmosDefender/" + nameof(CosmosSpell))]
    public class CosmosSpell : ScriptableObject
    {
        [SerializeField]
        private BaseSpell baseSpell;
        [SerializeField]
        private BaseSpell empoweredSpell;

        public bool isSpellEmpowered;

        public BaseSpell GetSpell() => isSpellEmpowered ? empoweredSpell : baseSpell;

        public void ApplyModifiers(IReadOnlyList<ISpellModifier> modifiers)
        {
            baseSpell.ApplyModifiers(modifiers);
            empoweredSpell.ApplyModifiers(modifiers);
        }
    }
}