using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(IncreaseAttackRangeModifier),
        menuName = "CosmosDefender/" + nameof(IncreaseAttackRangeModifier))]
    public class IncreaseAttackRangeModifier : BaseSpellModifier
    {
        public float extraRange;

        public override void Modify(ref SpellData data)
        {
            data.MaxAttackDistance += extraRange;
        }

        public override float GetInitialValue(SpellData data)
        {
            return data.MaxAttackDistance;
        }

        public override float GetFinalValue(SpellData data)
        {
            return data.MaxAttackDistance += extraRange;
        }
    }
}