using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(AttackAdditiveTemporalModifier), menuName = "CosmosDefender/" + nameof(AttackAdditiveTemporalModifier))]
    public class AttackAdditiveTemporalModifier : BaseTemporalAttributeModifier
    {
        [SerializeField]
        private float additiveDamage;

        public override void Modify(ref AttributesData data)
        {
            data.AttackDamage += additiveDamage;
        }
    }
}