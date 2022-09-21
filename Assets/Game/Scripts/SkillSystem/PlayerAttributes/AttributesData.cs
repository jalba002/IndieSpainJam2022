using System;
using UnityEngine;

namespace CosmosDefender
{
    [Serializable]
    public struct AttributesData : IOffensiveData, IDefensiveData, IMovementData, IReadOnlyOffensiveData, IReadOnlyDefensiveData, IReadOnlyMovementData
    {
        [SerializeField]
        private float attackDamage;
        [SerializeField]
        private float speed;
        [SerializeField]
        private float cooldownReduction;
        [SerializeField]
        private float maxHealth;
        [SerializeField]
        private float healthRegeneration;

        public float AttackDamage { get => attackDamage; set => attackDamage = value; }
        public float Speed { get => speed; set => speed = value; }
        public float CooldownReduction { get => cooldownReduction; set => cooldownReduction = value; }
        public float MaxHealth { get => maxHealth; set => maxHealth = value; }
        public float HealthRegeneration { get => healthRegeneration; set => healthRegeneration = value; }

        public AttributesData(float attackDamage, float speed, float maxHealth, float cooldownReduction, float healthRegeneration)
        {
            this.attackDamage = attackDamage;
            this.speed = speed;
            this.maxHealth = maxHealth;
            this.cooldownReduction = cooldownReduction;
            this.healthRegeneration = healthRegeneration;
        }
    }
}