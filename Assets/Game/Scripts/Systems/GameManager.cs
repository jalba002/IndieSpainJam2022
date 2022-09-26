using System;
using System.Collections.Generic;
using UnityEngine;
using CosmosDefender;
using TMPro;
using Sirenix.OdinInspector;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private PillarsConfig pillarsConfig;
    public List<Transform> WaypointsPaths1 = new List<Transform>();
    public List<Transform> WaypointsPaths2 = new List<Transform>();
    public List<Transform> WaypointsPaths3 = new List<Transform>();

    public List<Transform> AllWaypoints = new List<Transform>();

    protected override bool dontDestroyOnLoad => false;

    public StarResourceBehavior StarResourceBehavior;
    public GoddessResourceBehavior GoddessResourceBehavior;
    public ResourceManager ResourceManager;

    public List<PillarController> ActivePillars;
    public bool gameOver = false;

    [SerializeField]
    private CanvasFadeIn endScreen;
    [SerializeField]
    private TextMeshProUGUI endScreenText;
    [SerializeField]
    private TextMeshProUGUI endScreenSubtext;

    private PlayerInputs playerMenuInputs;

    public bool hasActivatedFirstPasivePillar;
    public bool hasActivatedFirstSkillPillar;
    public bool hasEmpoweredFirstPillar;
    public bool hasActivatedFirstGoddess;
    
    [Header("Tutorial")]
    [SerializeField] private TutorialConfig tutorial;

    [SerializeField] public EconomyConfig economyConfig;

    public Action OnGoddessMode;

    void Awake()
    {
        pillarsConfig.ClearObserverList();
        playerMenuInputs = FindObjectOfType<PlayerInputs>();
    }

    private void Start()
    {
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
    }

    [Button]
    public void EndGame(bool gameWon)
    {
        // Trigger endgame or something.

        // Tornar a l'escena inicial.
        gameOver = true;

        endScreen.FadeIn();
        playerMenuInputs.SetInputMap(PlayerInputMaps.UI);
        Time.timeScale = 0f;
        if (gameWon)
        {
            endScreenText.text = "¡Has ganado!";
        }
        else
        {
            endScreenText.text = "Has perdido";
        }

        endScreenSubtext.text = $"Tienes <color=green>{economyConfig.GetMoney()}</color> monedas acumuladas.\n\nPuedes gastarlas en <color=orange>mejoras permanentes</color> en la <color=yellow>tienda del menú principal</color>.";
    }

    public void ActivateGoddessMode()
    {
        if (!hasActivatedFirstGoddess)
        {
            hasActivatedFirstGoddess = true;
            TutorialPopUpManager.Instance.ActivateTutorial(tutorial, 2f);
        }

        foreach (var pillar in ActivePillars)
        {
            pillar.GoddessActive(ResourceManager.GetResourceData(ResourceType.Goddess).EffectDuration);
        }

        OnGoddessMode?.Invoke();
    }
}