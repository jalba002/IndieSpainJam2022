using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(SpellAreaIncreaseModifier), menuName = "CosmosDefender/Modifiers/" + nameof(SpellAreaIncreaseModifier))]
    public class SpellAreaIncreaseModifier : BaseSpellModifier
    {
        [SerializeField]
        private float timesBigger;

        public override void Modify(ref SpellData data)
        {
            data.UniformSize *= timesBigger;
        }
    }
}