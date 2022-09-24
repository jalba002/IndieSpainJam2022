using System.Collections.Generic;
using UnityEngine;
using CosmosDefender;

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

    void Awake()
    {
        pillarsConfig.ClearObserverList();
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
}