using UnityEngine;

namespace CosmosDefender
{
	public class MeleeEventReader : AnimationEventReader<MeleeAI>
	{
		public void ReadAttackSound()
		{
			attachedCreature.PlayAttackSoundEvent();
		}
	}
}