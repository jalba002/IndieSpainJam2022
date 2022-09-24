using UnityEngine;

[CreateAssetMenu(fileName = nameof(WaveConfig), menuName = "CosmosDefender/" + nameof(WaveConfig))]
public class WaveConfig : ScriptableObject
{
    public float timeBetweenEnemySpawn;
    public float timeForNextWave;
    public EnemyWaveConfig[] EnemyConfig;
    public int ShopCoinReward;
}

[System.Serializable]
public class EnemyWaveConfig
{
    public enum enemyTypes
    {
        Melee = 0,
        Ranged = 1,
        Commander = 2
    }
    public enum pathsToFollow
    {
        Left = 0,
        Middle = 1,
        Right = 2
    }

    public enemyTypes enemyType;
    public pathsToFollow pathToFollow;
}