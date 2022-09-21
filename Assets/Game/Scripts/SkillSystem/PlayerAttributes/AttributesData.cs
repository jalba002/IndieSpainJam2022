using System;
using UnityEngine;

namespace CosmosDefender
{
    [Serializable]
    public struct AttributesData : ICombatOffensiveData, IMovementData, IReadOnlyCombatData, IReadOnlySpeedData
    {
        [SerializeField]
        private float attackDamage;
        [SerializeField]
        private float speed;
        [SerializeField]
        private float maxHealth;

        public float AttackDamage { get => attackDamage; set => attackDamage = value; }
        public float Speed { get => speed; set => speed = value; }
        public float MaxHealth { get => maxHealth; set => maxHealth = value; }

        public AttributesData(float attackDamage, float speed, float maxHealth)
        {
            this.attackDamage = attackDamage;
            this.speed = speed;
            this.maxHealth = maxHealth;
        }
    }
}