using System;
using System.Collections.Generic;
using UnityEngine;
using CosmosDefender;
using TMPro;
using Sirenix.OdinInspector;
using Unity.VisualScripting;

public class GameManager : MonoSingleton<GameManager>
{
    public List<Transform> WaypointsPaths1 = new List<Transform>();
    public List<Transform> WaypointsPaths2 = new List<Transform>();
    public List<Transform> WaypointsPaths3 = new List<Transform>();

    public List<Transform> AllWaypoints = new List<Transform>();

    protected override bool dontDestroyOnLoad => false;

    public ResourceManager ResourceManager;

    public List<PillarController> ActivePillars;
    public bool gameOver = false;

    [SerializeField]
    private WinMenu endScreen;
    [SerializeField]
    private TextMeshProUGUI endScreenText;
    [SerializeField]
    private TextMeshProUGUI endScreenSubtext;

    private PlayerInputs playerMenuInputs;

    [Header("Tutorials")]
    // Please.... stop it...
    public bool hasActivatedBasicTutorial;
    public bool hasActivatedFirstPasivePillar;
    public bool hasActivatedFirstSkillPillar;
    public bool hasEmpoweredFirstPillar;
    public bool hasActivatedFirstGoddess;

    [Header("Tutorial")] [SerializeField] private TutorialConfig goddessTutorial;
    [Header("Tutorial")] [SerializeField] private TutorialConfig basicTutorial;

    [SerializeField] public EconomyConfig economyConfig;

    public Action OnGoddessMode;

    void Awake()
    {
        playerMenuInputs = FindObjectOfType<PlayerInputs>();
        // Get PlayerPrefs data.
    }
    

    void LoadPlayerPrefs()
    {
        Load(nameof(hasActivatedBasicTutorial),ref hasActivatedBasicTutorial);
        Load(nameof(hasActivatedFirstPasivePillar),ref hasActivatedFirstPasivePillar);
        Load(nameof(hasActivatedFirstSkillPillar),ref hasActivatedFirstSkillPillar);
        Load(nameof(hasEmpoweredFirstPillar),ref hasEmpoweredFirstPillar);
        Load(nameof(hasActivatedFirstGoddess),ref hasActivatedFirstGoddess);
    }

    void Load(string key, ref bool variable)
    {
        variable = PlayerPrefs.GetInt(key) >= 1 ? true : false;
        //Debug.Log($"{key} has a value of {variable}");
    }

    public void Save(string key, bool variable)
    {
        PlayerPrefs.SetInt(key, variable ? 1 : 0);
        Debug.Log($"Saving {key} with value {variable}");
        PlayerPrefs.Save();
    }

    private void Start()
    {
        LoadPlayerPrefs();

        foreach (var item in WaypointsPaths1)
        {
            AllWaypoints.Add(item);
        }

        foreach (var item in WaypointsPaths2)
        {
            AllWaypoints.Add(item);
        }

        foreach (var item in WaypointsPaths3)
        {
            AllWaypoints.Add(item);
        }

        if (hasActivatedBasicTutorial) return;
        TutorialPopUpManager.Instance.ActivateTutorial(basicTutorial, 3f);
        Save(nameof(hasActivatedBasicTutorial), hasActivatedBasicTutorial = true);
    }

    [Button]
    public void EndGame(bool gameWon)
    {
        // Trigger endgame or something.

        // Tornar a l'escena inicial.
        gameOver = true;

        endScreen.GetComponent<CanvasFadeIn>().FadeIn();
        playerMenuInputs.SetInputMap(PlayerInputMaps.UI);
        Time.timeScale = 0f;

        if(gameWon)
            endScreen.Win(economyConfig.GetMoney());
        else
            endScreen.Loss(economyConfig.GetMoney());
    }

    public void ActivateGoddessMode()
    {
        if (!hasActivatedFirstGoddess)
        {
            Save(nameof(hasActivatedFirstGoddess), hasActivatedFirstGoddess = true);
            TutorialPopUpManager.Instance.ActivateTutorial(goddessTutorial, 2f);
        }

        foreach (var pillar in ActivePillars)
        {
            pillar.GoddessActive(ResourceManager.GetResourceData(ResourceType.Goddess).EffectDuration);
        }
        
        // And then, switch all spells to cosmos mode.
        

        OnGoddessMode?.Invoke();
    }
}