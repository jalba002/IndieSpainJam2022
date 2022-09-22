using UnityEngine;

namespace CosmosDefender
{
	[CreateAssetMenu(fileName = nameof(MeteorSpellWeak), menuName = "CosmosDefender/"+nameof(MeteorSpellWeak))]
	public class MeteorSpellWeak : BaseSpell
	{
		public override void Cast(Transform spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData)
		{
			var instance = Instantiate(prefab, spawnPoint.position, rotation);
			instance.InstantiateBullet(spawnPoint, forward, rotation, combatData, currentData);
		}
	}
}