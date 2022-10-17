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

        public override float GetInitialValue(SpellData data)
        {
            return data.DamageMultiplier;
        }

        public override float GetFinalValue(SpellData data)
        {
            return data.DamageMultiplier *= attackMultiplier;
        }
    }
}