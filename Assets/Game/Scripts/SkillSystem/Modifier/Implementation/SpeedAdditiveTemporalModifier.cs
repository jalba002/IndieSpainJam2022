using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(SpeedAdditiveTemporalModifier),
        menuName = "CosmosDefender/Modifiers/" + nameof(SpeedAdditiveTemporalModifier))]
    public class SpeedAdditiveTemporalModifier : BaseTemporalAttributeModifier
    {
        [SerializeField] private float additiveSpeed;

        public override void Modify(ref AttributesData data)
        {
            data.Speed += additiveSpeed;
        }

        public override float GetInitialValue(AttributesData data)
        {
            return data.Speed;
        }

        public override float GetFinalValue(AttributesData data)
        {
            return data.Speed += additiveSpeed;
        }
    }
}