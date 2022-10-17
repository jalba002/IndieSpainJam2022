using FMODUnity;
using UnityEngine;

namespace CosmosDefender
{
    public class MeleeAI : EnemyAI
    {
        [SerializeField] private StudioEventEmitter attackSound;

        protected override void Attack()
        {
            base.Attack();
            //attackSound.Play();
        }

        public void PlayAttackSoundEvent()
        {
            attackSound.Play();
        }
    }
}