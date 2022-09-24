using UnityEngine;

namespace CosmosDefender
{
	[CreateAssetMenu(fileName = nameof(EnemyData), menuName = "CosmosDefender/"+nameof(EnemyData))]
    public class EnemyData : ScriptableObject, IReadOnlyOffensiveData
    {
        public float MaxHealth;
        public float Speed;
        public float attackDamage;
        public float attackRange;
        public float AggroRange = 10f;
        [Header("Defence")] 
        public float Defence;
        [Header("Resources")]
        public float StarResourceOnDeath;
        public float GoddessResourceOnDeath;

        public float AttackDamage => attackDamage;

        public float CooldownReduction => 0f;
    }
}