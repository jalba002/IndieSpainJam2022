using UnityEngine;

namespace CosmosDefender
{
	[CreateAssetMenu(fileName = nameof(MeteorSpellStrong), menuName = "CosmosDefender/"+nameof(MeteorSpellStrong))]
	public class MeteorSpellStrong : BaseSpell
	{
		public override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData)
		{
			var instance = Instantiate(prefab, spawnPoint, rotation);
			instance.InstantiateBullet(spawnPoint, forward, rotation, combatData, currentData);
		}
	}
}