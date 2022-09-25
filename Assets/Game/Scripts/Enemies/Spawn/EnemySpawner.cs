using System.Collections;
using CosmosDefender;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private WaveConfig[] waveConfigs;
    private int currentWave;

    [SerializeField]
    private Transform[] spawnPoints;
    [SerializeField]
    private GameObject[] enemyPrefabs;

    [SerializeField]
    private EconomyConfig economyConfig;

    private int currentWaveEnemies = 0;
    private bool firstPillarActivated = false;

    void Start()
    {
        currentWave = -1;
    }
    [Button]
    public void StartNextWave()
    {
        currentWave++;
        if (currentWave < waveConfigs.Length)
        {
            currentWaveEnemies = waveConfigs[currentWave].EnemyConfig.Length;
            StartCoroutine(EnemySpawnCoroutine(waveConfigs[currentWave]));
        }
    }

    public void PillarActivated()
    {
        if (firstPillarActivated)
            return;

        firstPillarActivated = true;
        StartNextWave();
    }

    public void DecreaseCurrentEnemyCounter()
    {
        currentWaveEnemies--;

        if (currentWaveEnemies <= 0)
        {
            FinishWave();
        }
    }

    private void FinishWave()
    {
        economyConfig.AddMoney(waveConfigs[currentWave].ShopCoinReward);
        StartCoroutine(NextWaveTimerCoroutine(waveConfigs[currentWave].timeForNextWave));
        if (currentWave >= waveConfigs.Length-1)
        {
            CronoScheduler.Instance.ScheduleForTime(2f, () => GameManager.Instance.EndGame(true));
        }
    }

    IEnumerator EnemySpawnCoroutine(WaveConfig currentWaveConfig)
    {
        foreach (var item in currentWaveConfig.EnemyConfig)
        {
            var enemy = Instantiate(enemyPrefabs[(int)item.enemyType], spawnPoints[(int)item.pathToFollow].position, spawnPoints[(int)item.pathToFollow].rotation);
            enemy.GetComponent<PathFollower>().SetPath(item);
            enemy.GetComponent<EnemyHealthManager>().SetEnemySpawner(this);
            yield return new WaitForSeconds(currentWaveConfig.timeBetweenEnemySpawn);
        }
        yield return new WaitForSeconds(currentWaveConfig.timeForNextWave);
    }

    IEnumerator NextWaveTimerCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        StartNextWave();
    }
}
