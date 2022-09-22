using UnityEngine;

namespace CosmosDefender
{
	[CreateAssetMenu(fileName = nameof(TornadoSpellWeak), menuName = "CosmosDefender/"+nameof(TornadoSpellWeak))]
	public class TornadoSpellWeak : BaseSpell
	{
		public override void Cast(Transform spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData)
		{
			var instance = Instantiate(prefab, spawnPoint.position, rotation);
			instance.InstantiateBullet(spawnPoint, forward, rotation, combatData, currentData);
		}
	}
}