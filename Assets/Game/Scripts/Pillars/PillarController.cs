using System;
using FMODUnity;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CosmosDefender
{
    public class PillarController : MonoBehaviour
    {
        public enum PillarStates
        {
            Inactive,
            Active,
            Empowered
        }

        [Header("Config")] public string PillarName;
        [SerializeField] private PillarStates pillarCurrentState;
        [Header("Translation")] public string tableReference = "Mecánicas";

        [Header("Configs")] [SerializeField]
        List<IPillarObserverModifier> pillarObserverModifiers = new List<IPillarObserverModifier>();

        private List<PillarObserver> observersInRange = new List<PillarObserver>();
        public PillarsConfig PillarConfig;

        private ResourceManager resourceManager;
        [SerializeField] private EnemySpawner enemySpawner;
        private Animator animator;
        private StudioEventEmitter activatedSoundRef;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, PillarConfig.Range);
            Gizmos.color = Color.white;
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            GetComponents(pillarObserverModifiers);
            resourceManager = GameManager.Instance.ResourceManager;
            activatedSoundRef = GetComponent<StudioEventEmitter>();
        }

        private void Start()
        {
            StartPillar();
        }

        void StartPillar()
        {
            switch (pillarCurrentState)
            {
                case PillarStates.Inactive:
                    // 
                    break;
                case PillarStates.Active:
                    // 
                    activatedSoundRef.Play();
                    GameManager.Instance.ActivePillars.Add(this);
                    animator.SetTrigger("Active");
                    break;
                case PillarStates.Empowered:
                    // 
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Update()
        {
            foreach (var observer in PillarConfig.PillarObservers)
            {
                var distanceFromObserver = Vector3.Distance(transform.position, observer.transform.position);

                if (observersInRange.Contains(observer))
                {
                    if (distanceFromObserver > PillarConfig.Range)
                    {
                        observersInRange.Remove(observer);
                        observer.RemovePillar(this);

                        if (pillarCurrentState != PillarStates.Inactive)
                        {
                            foreach (var pillar in pillarObserverModifiers)
                            {
                                pillar.OnObserverOutsideOfRange(observer);
                            }
                        }
                    }
                }
                else
                {
                    if (distanceFromObserver <= PillarConfig.Range)
                    {
                        observersInRange.Add(observer);
                        observer.AddPillar(this);

                        if (pillarCurrentState != PillarStates.Inactive)
                        {
                            foreach (var pillar in pillarObserverModifiers)
                            {
                                pillar.OnObserverInRange(observer);
                            }
                        }
                    }
                }
            }
        }

        [Button]
        public void ActivatePillar()
        {
            if (pillarCurrentState == PillarStates.Empowered)
                return;

            if (CanBeActivated(ResourceType.Stars))
            {
                resourceManager.DecreaseResource(ResourceType.Stars, PillarConfig.ActivateCost);
                enemySpawner.PillarActivated();
                activatedSoundRef.Play();
                GameManager.Instance.ActivePillars.Add(this);
                foreach (var pillar in pillarObserverModifiers)
                {
                    foreach (var observer in PillarConfig.PillarObservers)
                    {
                        pillar.OnPillarActivate(observer);
                    }
                }

                pillarCurrentState = PillarStates.Active;
                animator.SetTrigger("Active");
            }
            else if (CanBeEmpowered(ResourceType.Stars))
            {
                resourceManager.DecreaseResource(ResourceType.Stars, PillarConfig.EmpowerCost);
                foreach (var pillar in pillarObserverModifiers)
                {
                    foreach (var observer in PillarConfig.PillarObservers)
                    {
                        Debug.Log("Updating Empowers.");
                        pillar.SetPillarEmpowerState(observer, true);
                        activatedSoundRef.Play();
                        CronoScheduler.Instance.ScheduleForTime(PillarConfig.EmpoweredDuration, () =>
                        {
                            pillar.SetPillarEmpowerState(observer, false);
                            pillarCurrentState = PillarStates.Active;
                            animator.SetBool("Empowered", false);
                        });
                    }
                }

                pillarCurrentState = PillarStates.Empowered;
                animator.SetBool("Empowered", true);
            }
        }

        public void GoddessActive(float duration)
        {
            if (pillarCurrentState != PillarStates.Active)
                return;

            foreach (var pillar in pillarObserverModifiers)
            {
                foreach (var observer in PillarConfig.PillarObservers)
                {
                    pillar.OnGoddessActive(observer);

                    if (duration > 0)
                    {
                        CronoScheduler.Instance.ScheduleForTime
                        (
                            duration,
                            () => { pillar.SetPillarEmpowerState(observer, false); }
                        );
                    }
                }
            }

            animator.SetBool("GoddessMode", true);

            CronoScheduler.Instance.ScheduleForTime(duration, () => { GoddessDeactivated(); });
        }

        public void GoddessDeactivated()
        {
            animator.SetBool("GoddessMode", false);

            foreach (var pillar in pillarObserverModifiers)
            {
                foreach (var observer in PillarConfig.PillarObservers)
                {
                    pillar.OnGoddessUnactive(observer);
                }
            }

            pillarCurrentState = PillarStates.Active;
        }

        private bool CanBeEmpowered(ResourceType resource)
        {
            return resourceManager.HasEnoughResourceToSpend(resource, PillarConfig.EmpowerCost) &&
                   pillarCurrentState == PillarStates.Active;
        }

        private bool CanBeActivated(ResourceType resource)
        {
            return resourceManager.HasEnoughResourceToSpend(resource, PillarConfig.ActivateCost) &&
                   pillarCurrentState == PillarStates.Inactive;
        }

        public PillarStates GetCurrentState() => pillarCurrentState;

        public float GetActivationCost()
        {
            float value = -1f;
            switch (pillarCurrentState)
            {
                case PillarStates.Inactive:
                    value = PillarConfig.ActivateCost;
                    break;
                case PillarStates.Active:
                    value = PillarConfig.EmpowerCost;
                    break;
                case PillarStates.Empowered:
                    // NO
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return value;
        }
    }
}