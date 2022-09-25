using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CosmosDefender
{
    public class PathFollower : MonoBehaviour
    {
        private List<Transform> followedPath = new List<Transform>();

        private GameManager gameManager;

        [SerializeField]
        private float waypointReachedRadius = 2;
        private int currentWaypoint = 0;

        private NavMeshAgent navMeshAgent;
        [SerializeField] private EnemyData data;
        private Animator animator;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            gameManager = GameManager.Instance;
        }

        private void Start()
        {
            navMeshAgent.updateRotation = true;
            navMeshAgent.speed = data.Speed;
            //navMeshAgent.stoppingDistance = data.attackRange * 0.25f;
        }

        void Update()
        {
            if (navMeshAgent.updatePosition == false)
                return;

            animator.SetFloat("Speed", navMeshAgent.isStopped ? 0f : navMeshAgent.velocity.magnitude / navMeshAgent.speed);
            if (Vector3.Distance(followedPath[currentWaypoint].position, transform.position) < waypointReachedRadius)
            {
                MoveToNextWaypoint();
            }
        }

        public void SetPath(EnemyWaveConfig currentEnemyConfig)
        {
            switch (currentEnemyConfig.pathToFollow)
            {
                case EnemyWaveConfig.pathsToFollow.Left:
                    followedPath = gameManager.WaypointsPaths1;
                    break;
                case EnemyWaveConfig.pathsToFollow.Middle:
                    followedPath = gameManager.WaypointsPaths2;
                    break;
                case EnemyWaveConfig.pathsToFollow.Right:
                    followedPath = gameManager.WaypointsPaths3;
                    break;
            }
            navMeshAgent.destination = followedPath[currentWaypoint].position;
        }

        void MoveToNextWaypoint()
        {
            if (currentWaypoint == followedPath.Count - 1)
                return;
            currentWaypoint++;
            navMeshAgent.destination = followedPath[currentWaypoint].position;
        }

        [Button]
        public void StopFollowingPath()
        {
            navMeshAgent.updatePosition = false;
        }

        [Button]
        public void ReturnToPath()
        {
            navMeshAgent.updatePosition = true;
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
}