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
    private PlayerInputs playerMenuInputs;

    public bool hasActivatedFirstPasivePillar;
    public bool hasActivatedFirstSkillPillar;
    public bool hasEmpoweredFirstPillar;
    public bool hasActivatedFirstGoddess;

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
            endScreenText.text = "Has ganado...\nPor ahora";
        }
        else
        {
            endScreenText.text = "Has muerto...";
        }
    }

    public void ActivateGoddessMode()
    {
        if (!hasActivatedFirstGoddess)
        {
            hasActivatedFirstGoddess = true;
            TutorialPopUpManager.Instance.ActivateTutorial(4, 2f);
        }

        foreach (var pillar in ActivePillars)
        {
            pillar.GoddessActive(ResourceManager.GetResourceData(ResourceType.Goddess).EffectDuration);
        }
    }
}