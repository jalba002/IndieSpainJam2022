using System;
using UnityEngine;

namespace CosmosDefender
{
	[CreateAssetMenu(fileName = nameof(ResourceConfig), menuName = "CosmosDefender/"+nameof(ResourceConfig))]
	public class ResourceConfig : ScriptableObject
	{
		public ResourceType ResourceType;
		public ResourceData baseResource;
	}

	[Serializable]
	public struct ResourceData
    {
		public float MaxResource;
		public float CurrentResource;
		public float StartingResource;
		public float ResourceOverTime;
		public float ResourcePerKill;
	}
}