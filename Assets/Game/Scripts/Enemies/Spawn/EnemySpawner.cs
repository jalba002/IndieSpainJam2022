using System;
using System.Collections;
using CosmosDefender;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private WaveSettings _waveSettings;
    [SerializeField] private WaveSettings _testSettings;

    private WaveSettings _currentSettings;
    
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
#if UNITY_EDITOR
        _currentSettings = Instantiate(_testSettings);
#else
        _currentSettings = Instantiate(_waveSettings);
#endif
        currentWave = -1;
    }
    
    public void StartNextWave()
    {
        BeforeCombatMusicRef.Stop();
        CombatMusicRef.Play();
        currentWave++;
        if (currentWave < _currentSettings.GetWaves().Length)
        {
            currentWaveEnemies = _currentSettings.GetWaves()[currentWave].EnemyConfig.Length;
            StartCoroutine(EnemySpawnCoroutine(_currentSettings.GetWaves()[currentWave]));
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
        economyConfig.AddMoney(_currentSettings.GetWaves()[currentWave].ShopCoinReward);
        
        if (currentWave >= _currentSettings.GetMaxWaves())
        {
            CronoScheduler.Instance.ScheduleForTime(2f, () => GameManager.Instance.EndGame(true));
            FinishWaveMusicRef.Stop();
            WinSoundRef.Play();
            OnWaveEnd?.Invoke(-1f);
            return;
        }
        
        float time = _currentSettings.GetWaves()[currentWave].timeForNextWave;
        StartCoroutine(NextWaveTimerCoroutine(time));
        OnWaveEnd?.Invoke(time);
        
        FinishWaveMusicRef.Play();
        CombatMusicRef.Stop();
    }

    IEnumerator EnemySpawnCoroutine(WaveConfig currentWaveConfig)
    {
        yield return new WaitForSecondsRealtime(1f);
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
