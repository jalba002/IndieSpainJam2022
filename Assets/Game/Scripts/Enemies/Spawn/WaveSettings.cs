using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CosmosDefender
{
	[CreateAssetMenu(fileName = nameof(WaveSettings), menuName = "CosmosDefender/"+nameof(WaveSettings))]
	public class WaveSettings : ScriptableObject
	{
		[SerializeField, InlineEditor]
		private WaveConfig[] waveConfigs;

		[SerializeField] private int MaxWaves = 5;

		public WaveConfig[] GetWaves()
		{
			return waveConfigs;
		}

		public void ApplyWaveModifiers<T>(List<T> modifiers)
		{
			// TODO apply wave modifiers.
			foreach (var mod in modifiers)
			{
				//mod.ApplyModifier(ref MaxWaves);
			}
		}

		public int GetMaxWaves()
		{
			return MaxWaves;
		}
		
		// Apply a wave modifier?
		// Add a flat amount of waves?
	}
}