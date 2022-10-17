using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(CooldownReductionAdditiveTemporalModifier),
        menuName = "CosmosDefender/Modifiers/" + nameof(CooldownReductionAdditiveTemporalModifier))]
    public class CooldownReductionAdditiveTemporalModifier : BaseTemporalAttributeModifier
    {
        [SerializeField] private float cooldownReductionAdditive;

        public override void Modify(ref AttributesData data)
        {
            data.CooldownReduction += cooldownReductionAdditive;
        }

        public override float GetInitialValue(AttributesData data)
        {
            return data.CooldownReduction;
        }

        public override float GetFinalValue(AttributesData data)
        {
            return data.CooldownReduction += cooldownReductionAdditive;
        }
    }
}