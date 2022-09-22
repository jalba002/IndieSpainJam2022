using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFollower : MonoBehaviour
{
    private List<Transform> followedPath;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private float waypointReachedRadius = 2;
    private int currentWaypoint = 0;

    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        followedPath = gameManager.WaypointsPaths1;
        navMeshAgent.destination = followedPath[currentWaypoint].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(followedPath[currentWaypoint].position, transform.position) < waypointReachedRadius)
        {
            MoveToNextWaypoint();
        }
    }

    void MoveToNextWaypoint()
    {
        currentWaypoint++;
        navMeshAgent.destination = followedPath[currentWaypoint].position;
    }

    [Button]
    public void StopFollowingPath()
    {
        navMeshAgent.destination = transform.position;
    }

    [Button]
    public void ReturnToPath()
    {
        var closestWaypoint = gameManager.AllWaypoints[0];
        var closestWaypointDistance = Vector3.Distance(closestWaypoint.position, transform.position);
        foreach (var item in gameManager.AllWaypoints)
        {
            var newWaypointDistance = Vector3.Distance(item.position, transform.position);
            if (newWaypointDistance < closestWaypointDistance)
            {
                closestWaypoint = item;
                closestWaypointDistance = newWaypointDistance;
            }
        }

        for (int i = 0; i < gameManager.WaypointsPaths1.Count; i++)
        {
            if (gameManager.WaypointsPaths1[i] == closestWaypoint)
            {
                currentWaypoint = i;
                followedPath = gameManager.WaypointsPaths1;
            }
        }
        for (int i = 0; i < gameManager.WaypointsPaths2.Count; i++)
        {
            if (gameManager.WaypointsPaths2[i] == closestWaypoint)
            {
                currentWaypoint = i;
                followedPath = gameManager.WaypointsPaths2;
            }
        }
        for (int i = 0; i < gameManager.WaypointsPaths3.Count; i++)
        {
            if (gameManager.WaypointsPaths3[i] == closestWaypoint)
            {
                currentWaypoint = i;
                followedPath = gameManager.WaypointsPaths3;
            }
        }

        navMeshAgent.destination = closestWaypoint.position;
    }
}