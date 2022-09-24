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

        public string PillarName;

        public PillarStates pillarCurrentState;
        [SerializeField]
        List<IPillarObserverModifier> pillarObserverModifiers = new List<IPillarObserverModifier>(); 

        private List<PillarObserver> observersInRange = new List<PillarObserver>();
        public PillarsConfig PillarConfig;

        [SerializeField]
        private ResourceData starResourceData;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, PillarConfig.Range);
            Gizmos.color = Color.white;
        }

        private void Awake()
        {
            GetComponents(pillarObserverModifiers);
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

            if (CanBeActivated(starResourceData.CurrentResource))
            {
                starResourceData.DecreaseResource(PillarConfig.ActivateCost);
                foreach (var pillar in pillarObserverModifiers)
                {
                    foreach (var observer in PillarConfig.PillarObservers)
                    {
                        pillar.OnPillarActivate(observer);
                    }
                }
                pillarCurrentState = PillarStates.Active;
            }
            else if(CanBeEmpowered(starResourceData.CurrentResource))
            {
                starResourceData.DecreaseResource(PillarConfig.EmpowerCost);
                foreach (var pillar in pillarObserverModifiers)
                {
                    foreach (var observer in PillarConfig.PillarObservers)
                    {
                        pillar.SetPillarEmpowerState(observer, true);
                        CronoScheduler.Instance.ScheduleForTime(PillarConfig.EmpoweredDuration, () => 
                        {
                            pillar.SetPillarEmpowerState(observer, false);
                            pillarCurrentState = PillarStates.Active;
                        });
                    }
                }
                pillarCurrentState = PillarStates.Empowered;
            }
        }

        private bool CanBeEmpowered(float currentResources)
        {
            return currentResources >= PillarConfig.EmpowerCost && pillarCurrentState == PillarStates.Active;
        }

        private bool CanBeActivated(float currentResources)
        {
            return currentResources >= PillarConfig.ActivateCost && pillarCurrentState == PillarStates.Inactive;
        }
    }
}