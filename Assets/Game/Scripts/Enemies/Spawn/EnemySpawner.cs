using System;
using System.Collections;
using CosmosDefender;
using FMODUnity;
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

    [SerializeField] private StudioEventEmitter BeforeCombatMusicRef;
    [SerializeField] private StudioEventEmitter FinishWaveMusicRef;
    [SerializeField] private StudioEventEmitter CombatMusicRef;
    [SerializeField] private StudioEventEmitter WinSoundRef;

    // A
    public Action OnWaveStart;
    // Time for next wave.
    public Action<float> OnWaveEnd;

    void Start()
    {
        currentWave = -1;
    }
    
    public void StartNextWave()
    {
        BeforeCombatMusicRef.Stop();
        CombatMusicRef.Play();
        currentWave++;
        if (currentWave < waveConfigs.Length)
        {
            currentWaveEnemies = waveConfigs[currentWave].EnemyConfig.Length;
            StartCoroutine(EnemySpawnCoroutine(waveConfigs[currentWave]));
        }
        OnWaveStart?.Invoke();
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
        
        float time = waveConfigs[currentWave].timeForNextWave;
        StartCoroutine(NextWaveTimerCoroutine(time));
        OnWaveEnd?.Invoke(time);
        
        FinishWaveMusicRef.Play();
        CombatMusicRef.Stop();
        if (currentWave >= waveConfigs.Length-1)
        {
            CronoScheduler.Instance.ScheduleForTime(2f, () => GameManager.Instance.EndGame(true));
            FinishWaveMusicRef.Stop();
            WinSoundRef.Play();
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
