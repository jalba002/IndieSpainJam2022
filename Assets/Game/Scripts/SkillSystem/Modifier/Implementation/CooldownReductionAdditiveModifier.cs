using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(CooldownReductionAdditiveModifier),
        menuName = "CosmosDefender/Modifiers/" + nameof(CooldownReductionAdditiveModifier))]
    public class CooldownReductionAdditiveModifier : BaseAttributeModifier
    {
        [SerializeField] private float additiveCooldownReduction;

        public override void Modify(ref AttributesData data)
        {
            data.CooldownReduction += additiveCooldownReduction;
        }

        public override float GetInitialValue(AttributesData data)
        {
            return data.CooldownReduction;
        }

        public override float GetFinalValue(AttributesData data)
        {
            return data.CooldownReduction += additiveCooldownReduction;
        }
    }
}