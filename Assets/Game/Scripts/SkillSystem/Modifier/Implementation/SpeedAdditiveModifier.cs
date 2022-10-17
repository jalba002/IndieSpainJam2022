using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(SpeedAdditiveModifier),
        menuName = "CosmosDefender/Modifiers/" + nameof(SpeedAdditiveModifier))]
    public class SpeedAdditiveModifier : BaseAttributeModifier
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