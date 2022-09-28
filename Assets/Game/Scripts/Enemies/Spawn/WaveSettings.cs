using Sirenix.OdinInspector;
using UnityEngine;

namespace CosmosDefender
{
	[CreateAssetMenu(fileName = nameof(WaveSettings), menuName = "CosmosDefender/"+nameof(WaveSettings))]
	public class WaveSettings : ScriptableObject
	{
		[SerializeField, InlineEditor]
		private WaveConfig[] waveConfigs;

		public WaveConfig[] GetWaves()
		{
			return waveConfigs;
		}
	}
}