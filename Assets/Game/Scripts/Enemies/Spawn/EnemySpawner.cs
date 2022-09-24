using Sirenix.OdinInspector;
using System.Collections;
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

    private int currentWaveEnemies = 0;

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
        else
        {
            Debug.Log("Game finished");
        }
    }

    public void DecreaseCurrentEnemyCounter()
    {
        currentWaveEnemies--;
        if (currentWaveEnemies <= 0)
        {
            StartCoroutine(NextWaveTimerCoroutine(waveConfigs[currentWave].timeForNextWave));
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
