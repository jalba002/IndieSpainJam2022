using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(SpellDurationIncreaseModifier), menuName = "CosmosDefender/Modifiers/" + nameof(SpellDurationIncreaseModifier))]
    public class SpellDurationIncreaseModifier : BaseSpellModifier
    {
        [SerializeField]
        private float increasedTime = 0;

        public override void Modify(ref SpellData data)
        {
            data.ActiveDuration += increasedTime;
        }
    }
}