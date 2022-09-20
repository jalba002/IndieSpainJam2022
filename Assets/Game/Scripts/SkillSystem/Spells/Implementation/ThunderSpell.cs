using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(ThunderSpell), menuName = "CosmosDefender/" + nameof(ThunderSpell))]
    public class ThunderSpell : BaseSpell
    {
        [SerializeField]
        private int amountOfMissiles;

        public override void Cast(Transform spawnPoint, IReadOnlyCombatData combatData)
        {
        }
    }
}