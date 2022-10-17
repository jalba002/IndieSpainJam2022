using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(AttackAdditiveTemporalModifier), menuName = "CosmosDefender/Modifiers/" + nameof(AttackAdditiveTemporalModifier))]
    public class AttackAdditiveTemporalModifier : BaseTemporalAttributeModifier
    {
        [SerializeField]
        private float additiveDamage;

        public override void Modify(ref AttributesData data)
        {
            data.AttackDamage += additiveDamage;
        }

        public override float GetInitialValue(AttributesData data)
        {
            return data.AttackDamage;
        }

        public override float GetFinalValue(AttributesData data)
        {
            return data.AttackDamage += additiveDamage;
        }
    }
}