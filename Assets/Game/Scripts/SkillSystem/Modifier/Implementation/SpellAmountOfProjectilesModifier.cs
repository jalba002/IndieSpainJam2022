using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(SpellAmountOfProjectilesModifier), menuName = "CosmosDefender/" + nameof(SpellAmountOfProjectilesModifier))]
    public class SpellAmountOfProjectilesModifier : BaseSpellModifier
    {
        public int amountToAdd;

        public override void Modify(ref SpellData data)
        {
            data.Amount += amountToAdd;
        }
    }
}