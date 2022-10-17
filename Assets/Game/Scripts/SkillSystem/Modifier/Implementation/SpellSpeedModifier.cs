using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(SpellSpeedModifier), menuName = "CosmosDefender/" + nameof(SpellSpeedModifier))]
    public class SpellSpeedModifier : BaseSpellModifier
    {
        public float speed;

        public override void Modify(ref SpellData data)
        {
            data.Speed += speed;
        }

        public override float GetInitialValue(SpellData data)
        {
            return data.Speed;
        }

        public override float GetFinalValue(SpellData data)
        {
            return data.Speed *= speed;
        }
    }
}