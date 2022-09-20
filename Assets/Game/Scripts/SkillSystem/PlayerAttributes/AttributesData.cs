using System;
using UnityEngine;

namespace CosmosDefender
{
    [Serializable]
    public struct AttributesData : ICombatData, ISpeedData, IReadOnlyCombatData, IReadOnlySpeedData
    {
        [SerializeField]
        private float attackDamage;
        [SerializeField]
        private float speed;

        public float AttackDamage { get => attackDamage; set => attackDamage = value; }
        public float Speed { get => speed; set => speed = value; }

        public AttributesData(float attackDamage, float speed)
        {
            this.attackDamage = attackDamage;
            this.speed = speed;
        }
    }
}