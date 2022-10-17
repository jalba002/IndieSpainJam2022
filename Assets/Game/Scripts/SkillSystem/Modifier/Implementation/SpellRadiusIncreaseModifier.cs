using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(SpellRadiusIncreaseModifier),
        menuName = "CosmosDefender/Modifiers/" + nameof(SpellRadiusIncreaseModifier))]
    public class SpellRadiusIncreaseModifier : BaseSpellModifier
    {
        [SerializeField] private float timesBigger;

        public override void Modify(ref SpellData data)
        {
            data.ProjectileRadius += timesBigger;
        }

        public override float GetInitialValue(SpellData data)
        {
            return data.ProjectileRadius;
        }

        public override float GetFinalValue(SpellData data)
        {
            return data.ProjectileRadius += timesBigger;
        }
    }
}