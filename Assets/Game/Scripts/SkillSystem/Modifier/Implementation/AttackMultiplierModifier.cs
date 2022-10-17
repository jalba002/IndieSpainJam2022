using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(AttackMultiplierModifier), menuName = "CosmosDefender/Modifiers/" + nameof(AttackMultiplierModifier))]
    public class AttackMultiplierModifier : BaseAttributeModifier
    {
        [SerializeField]
        private float multiplierValue;

        public override void Modify(ref AttributesData data)
        {
            data.AttackDamage *= multiplierValue;
        }

        public override float GetInitialValue(AttributesData data)
        {
            return data.AttackDamage;
        }

        public override float GetFinalValue(AttributesData data)
        {
            return data.AttackDamage *= multiplierValue;
        }
    }
}