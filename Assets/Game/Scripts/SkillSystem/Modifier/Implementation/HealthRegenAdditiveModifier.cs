using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(HealthRegenAdditiveModifier), menuName = "CosmosDefender/Modifiers/" + nameof(HealthRegenAdditiveModifier))]
    public class HealthRegenAdditiveModifier : BaseAttributeModifier
    {
        [SerializeField]
        private float additiveHealthRegeneration;

        public override void Modify(ref AttributesData data)
        {
            data.HealthRegeneration += additiveHealthRegeneration;
        }
    }
}