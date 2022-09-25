using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(HealthRegenAdditiveTemporalModifier), menuName = "CosmosDefender/Modifiers/" + nameof(HealthRegenAdditiveTemporalModifier))]
    public class HealthRegenAdditiveTemporalModifier : BaseTemporalAttributeModifier
    {
        [SerializeField]
        private float healthRegenerationSpeed;

        public override void Modify(ref AttributesData data)
        {
            data.HealthRegeneration += healthRegenerationSpeed;
        }
    }
}