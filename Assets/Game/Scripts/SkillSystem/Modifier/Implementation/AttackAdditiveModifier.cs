using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(AttackAdditiveModifier), menuName = "CosmosDefender/" + nameof(AttackAdditiveModifier))]
    public class AttackAdditiveModifier : BaseAttributeModifier
    {
        [SerializeField]
        private float additiveDamage;

        public override void Modify(ref AttributesData data)
        {
            data.AttackDamage += additiveDamage;
        }
    }
}