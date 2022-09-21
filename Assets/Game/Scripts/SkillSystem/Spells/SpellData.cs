using System;
using UnityEngine;

namespace CosmosDefender
{
    [Serializable]
    public struct SpellData
    {
        [SerializeField]
        private float damageMultiplier;
        [SerializeField]
        private float cooldown;
        [SerializeField]
        private float amount;
        [SerializeField]
        private float speed;
        [SerializeField]
        private float uniformSize;
        [SerializeField]
        private float maxAttackDistance;
        [SerializeField]
        private float lifetime;
        [SerializeField]
        private float activeDuration;

        public float DamageMultiplier => damageMultiplier;
        public float Cooldown => cooldown;
        public float Amount => amount;
        public float Speed => speed;
        public float UniformSize => uniformSize;
        public float MaxAttackDistance => maxAttackDistance;
        public float Lifetime => lifetime;
        public float ActiveDuration => activeDuration;
    }
}