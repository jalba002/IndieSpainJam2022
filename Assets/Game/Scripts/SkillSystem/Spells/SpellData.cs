using System;
using UnityEngine;

namespace CosmosDefender
{
    [Serializable]
    public struct SpellData
    {
        [SerializeField]
        private SpellType spellType;
        [SerializeField]
        private float damageMultiplier;
        [SerializeField]
        private float cooldown;
        [SerializeField]
        private int amount;
        [SerializeField]
        private float speed;
        [SerializeField]
        private float uniformSize;
        [SerializeField] 
        private float projectileRadius;
        [SerializeField] 
        private float projectileDelay;
        [SerializeField]
        private float maxAttackDistance;
        [SerializeField]
        private float lifetime;
        [SerializeField]
        private float activeDuration;

        public SpellType SpellType => spellType;
        public float DamageMultiplier { get => damageMultiplier; set => damageMultiplier = value; }
        public float Cooldown { get => cooldown; set => cooldown = value; }
        public int Amount { get => amount; set => amount = value; }
        public float Speed { get => speed; set => speed = value; }
        public float UniformSize { get => uniformSize; set => uniformSize = value; }
        public float ProjectileRadius { get => projectileRadius; set => projectileRadius = value; }
        public float ProjectileDelay { get => projectileDelay; set => projectileDelay = value; }
        public float MaxAttackDistance { get => maxAttackDistance; set => maxAttackDistance = value; }
        public float Lifetime { get => lifetime; set => lifetime = value; }
        public float ActiveDuration { get => activeDuration; set => activeDuration = value; }
    }
}