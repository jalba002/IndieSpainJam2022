using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(SpellAttackMultiplierModifier), menuName = "CosmosDefender/" + nameof(SpellAttackMultiplierModifier))]
    public class SpellAttackMultiplierModifier : BaseSpellModifier
    {
        public float attackMultiplier;

        public override void Modify(ref SpellData data)
        {
            data.DamageMultiplier *= attackMultiplier;
        }
    }
}