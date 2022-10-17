using System;
using System.Collections;
using CosmosDefender;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Settings")] [SerializeField]
    private WaveSettings _waveSettings;

    [SerializeField] private WaveSettings _testSettings;


    [Header("SpawnPoints")] [SerializeField]
    private Transform[] spawnPoints;

    [Header("Prefabs")] [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Resources")] [SerializeField] private EconomyConfig economyConfig;

    [Header("Music")] [SerializeField] private StudioEventEmitter BeforeCombatMusicRef;
    [SerializeField] private StudioEventEmitter FinishWaveMusicRef;
    [SerializeField] private StudioEventEmitter CombatMusicRef;
    [SerializeField] private StudioEventEmitter WinSoundRef;

    [Header("Private Stuff")] private WaveSettings _currentSettings;

    private int currentWaveEnemies = 0;

    private int currentWave = 0;

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

    private void FinishWave()
    {
        economyConfig.AddMoney(_currentSettings.GetWaves()[currentWave].ShopCoinReward);

        // If the next wave were to finish it all. Ignore.
        if (currentWave >= _currentSettings.GetWaves().Length - 1)
        {
            CronoScheduler.Instance.ScheduleForTime(5f, () => GameManager.Instance.EndGame(true));
            BeforeCombatMusicRef.Stop();
            CombatMusicRef.Stop();
            FinishWaveMusicRef.Stop();
            WinSoundRef.Play();
            //OnWaveEnd?.Invoke(5f);
            return;
        }

        float time = _currentSettings.GetWaves()[currentWave].timeForNextWave;
        StartCoroutine(NextWaveTimerCoroutine(time));

        OnWaveEnd?.Invoke(time);

        currentWave++;

        FinishWaveMusicRef.Play();
        CombatMusicRef.Stop();
    }

    IEnumerator EnemySpawnCoroutine(WaveConfig currentWaveConfig)
    {
        foreach (var item in currentWaveConfig.EnemyConfig)
        {
            var enemy = Instantiate(enemyPrefabs[(int) item.enemyType], spawnPoints[(int) item.pathToFollow].position,
                spawnPoints[(int) item.pathToFollow].rotation);
            enemy.GetComponent<PathFollower>().SetPath(item);
            enemy.GetComponent<EnemyHealthManager>().Initialize(this);
            yield return new WaitForSeconds(currentWaveConfig.timeBetweenEnemySpawn);
        }
    }

    IEnumerator NextWaveTimerCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        StartNextWave();
    }

    public void StartNextWave()
    {
        BeforeCombatMusicRef.Stop();
        CombatMusicRef.Play();

        currentWaveEnemies = _currentSettings.GetWaves()[currentWave].EnemyConfig.Length;
        StartCoroutine(EnemySpawnCoroutine(_currentSettings.GetWaves()[currentWave]));

        OnWaveStart?.Invoke();
    }

    #region External

    public void DecreaseCurrentEnemyCounter()
    {
        currentWaveEnemies--;

        if (currentWaveEnemies <= 0)
        {
            FinishWave();
        }
    }

    public void PillarActivated()
    {
        if (currentWave != -1) return;
        currentWave = 0;
        StartNextWave();
    }

    #endregion
}