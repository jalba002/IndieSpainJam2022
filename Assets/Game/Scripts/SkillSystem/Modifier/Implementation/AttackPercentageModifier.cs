using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(AttackPercentageModifier), menuName = "CosmosDefender/Modifiers/" + nameof(AttackPercentageModifier))]
    public class AttackPercentageModifier : BaseAttributeModifier
    {
        [SerializeField]
        private float additivePercentage;

        public override void Modify(ref AttributesData data)
        {
            data.AttackDamage += (data.AttackDamage * additivePercentage) / 100;
        }
    }
}