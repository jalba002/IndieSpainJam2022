using UnityEngine;

namespace CosmosDefender
{
	[CreateAssetMenu(fileName = nameof(EnemyData), menuName = "CosmosDefender/"+nameof(EnemyData))]
    public class EnemyData : ScriptableObject
    {
        public float MaxHealth;
        public float Speed;
        public float AttackDamage;
        public float AttackRange;
        public float StarResourceOnDeath;
        public float GoddessResourceOnDeath;
    }
}