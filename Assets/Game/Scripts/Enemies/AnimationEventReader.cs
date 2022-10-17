using UnityEngine;

namespace CosmosDefender
{
	public class AnimationEventReader<T> : MonoBehaviour where T: EnemyAI
	{
		[SerializeField] protected T attachedCreature;
	}
}