using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(HealthRegenAdditiveModifier),
        menuName = "CosmosDefender/Modifiers/" + nameof(HealthRegenAdditiveModifier))]
    public class HealthRegenAdditiveModifier : BaseAttributeModifier
    {
        [SerializeField] private float additiveHealthRegeneration;

        public override void Modify(ref AttributesData data)
        {
            data.HealthRegeneration += additiveHealthRegeneration;
        }

        public override float GetInitialValue(AttributesData data)
        {
            return data.HealthRegeneration;
        }

        public override float GetFinalValue(AttributesData data)
        {
            return data.HealthRegeneration += additiveHealthRegeneration;
        }
    }
}