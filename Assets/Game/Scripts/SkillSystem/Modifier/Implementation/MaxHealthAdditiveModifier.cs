using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(MaxHealthAdditiveModifier), menuName = "CosmosDefender/Modifiers/" + nameof(MaxHealthAdditiveModifier))]
    public class MaxHealthAdditiveModifier : BaseAttributeModifier
    {
        [SerializeField]
        private float additiveMaxHealth;

        public override void Modify(ref AttributesData data)
        {
            data.MaxHealth += additiveMaxHealth;
        }
    }
}