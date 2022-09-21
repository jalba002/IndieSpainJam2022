using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(SpeedAdditiveModifier), menuName = "CosmosDefender/" + nameof(SpeedAdditiveModifier))]
    public class SpeedAdditiveModifier : BaseAttributeModifier
    {
        [SerializeField]
        private float additiveSpeed;

        public override void Modify(ref AttributesData data)
        {
            data.Speed += additiveSpeed;
        }
    }
}