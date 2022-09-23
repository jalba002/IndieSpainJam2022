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
            StartCoroutine(EnemySpawnCoroutine(waveConfigs[currentWave]));
        }
        else
        {
            Debug.Log("Game finished");
        }
    }

    IEnumerator EnemySpawnCoroutine(WaveConfig currentWaveConfig)
    {
        foreach (var item in currentWaveConfig.EnemyConfig)
        {
            var enemy = Instantiate(enemyPrefabs[(int)item.enemyType], spawnPoints[(int)item.pathToFollow].position, spawnPoints[(int)item.pathToFollow].rotation);
            enemy.GetComponent<PathFollower>().SetPath(item);
            yield return new WaitForSeconds(currentWaveConfig.timeBetweenEnemySpawn);
        }
        yield return new WaitForSeconds(currentWaveConfig.timeForNextWave);
        StartNextWave();
    }
}
