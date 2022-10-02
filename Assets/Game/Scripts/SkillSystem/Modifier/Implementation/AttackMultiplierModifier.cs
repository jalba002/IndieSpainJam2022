using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(AttackMultiplierModifier), menuName = "CosmosDefender/Modifiers/" + nameof(AttackMultiplierModifier))]
    public class AttackMultiplierModifier : BaseAttributeModifier
    {
        [SerializeField]
        private float additiveDamage;

        public override void Modify(ref AttributesData data)
        {
            data.AttackDamage *= additiveDamage;
        }
    }
}