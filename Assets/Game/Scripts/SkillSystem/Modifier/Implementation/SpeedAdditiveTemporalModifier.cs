using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(SpeedAdditiveTemporalModifier), menuName = "CosmosDefender/Modifiers/" + nameof(SpeedAdditiveTemporalModifier))]
    public class SpeedAdditiveTemporalModifier : BaseTemporalAttributeModifier
    {
        [SerializeField]
        private float additiveSpeed;

        public override void Modify(ref AttributesData data)
        {
            data.Speed += additiveSpeed;
        }
    }
}