using UnityEngine;

namespace CosmosDefender
{
	[CreateAssetMenu(fileName = nameof(ResourceData), menuName = "CosmosDefender/"+nameof(ResourceData))]
	public class ResourceData : ScriptableObject
	{
		public ResourceType ResourceType;
		public float MaxResource;
		public float StartingResource;
		public float ResourceOverTime;
	}
}