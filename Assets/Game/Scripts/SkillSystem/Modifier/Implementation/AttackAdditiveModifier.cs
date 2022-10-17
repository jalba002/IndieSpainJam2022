using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(AttackAdditiveModifier), menuName = "CosmosDefender/Modifiers/" + nameof(AttackAdditiveModifier))]
    public class AttackAdditiveModifier : BaseAttributeModifier
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