using UnityEngine;

namespace CosmosDefender
{
	[CreateAssetMenu(fileName = nameof(ResourceData), menuName = "CosmosDefender/"+nameof(ResourceData))]
	public class ResourceData : ScriptableObject
	{
		public ResourceType ResourceType;
		public float MaxResource;
		public float CurrentResource;
		public float StartingResource;
		public float ResourceOverTime;
		public float ResourcePerKill;

		public void Initialize()
        {
			CurrentResource = StartingResource;
		}

		public void IncreaseResource(float value)
        {
			CurrentResource += value;
			CurrentResource = Mathf.Clamp(CurrentResource, 0, MaxResource);
        }

		public void DecreaseResource(float value)
		{
			CurrentResource -= value;
			CurrentResource = Mathf.Clamp(CurrentResource, 0, MaxResource);
		}

		public bool HasEnoughResource(float cost)
        {
			return CurrentResource >= cost;
		}
	}
}