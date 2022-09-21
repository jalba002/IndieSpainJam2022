using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(SpellSizeIncreaserModifier), menuName = "CosmosDefender/" + nameof(SpellSizeIncreaserModifier))]
    public class SpellSizeIncreaserModifier : BaseSpellModifier
    {
        [SerializeField]
        private float timesBigger;

        public override void Modify(ref SpellData data)
        {
            data.UniformSize *= timesBigger;
        }
    }
}