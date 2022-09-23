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

        public PillarStates pillarCurrentState;
        [SerializeField]
        List<IPillarObserverModifier> pillarObserverModifiers = new List<IPillarObserverModifier>(); 

        private List<PillarObserver> observersInRange = new List<PillarObserver>();
        [SerializeField]
        private PillarsConfig pillarConfig;

        private StarResourceBehavior starResourceBehavior;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, pillarConfig.Range);
            Gizmos.color = Color.white;
        }

        private void Awake()
        {
            GetComponents(pillarObserverModifiers);
            starResourceBehavior = GameManager.Instance.StarResourceBehavior;
        }

        private void Update()
        {
            if (pillarCurrentState == PillarStates.Inactive)
                return;

            foreach (var observer in pillarConfig.PillarObservers)
            {
                var distanceFromObserver = Vector3.Distance(transform.position, observer.transform.position);

                if (observersInRange.Contains(observer))
                {
                    if (distanceFromObserver > pillarConfig.Range)
                    {
                        foreach (var pillar in pillarObserverModifiers)
                        {
                            pillar.OnObserverOutsideOfRange(observer);
                        }
                        observersInRange.Remove(observer);
                    }
                }
                else
                {
                    if (distanceFromObserver <= pillarConfig.Range)
                    {
                        foreach (var pillar in pillarObserverModifiers)
                        {
                            pillar.OnObserverInRange(observer);
                        }
                        observersInRange.Add(observer);
                    }
                }
            }
        }

        [Button]
        public void ActivatePillar()
        {
            if (pillarCurrentState == PillarStates.Empowered)
                return;

            if (CanBeActivated(starResourceBehavior.GetCurrentResource()))
            {
                starResourceBehavior.DecreaseResource(pillarConfig.ActivateCost);
                foreach (var pillar in pillarObserverModifiers)
                {
                    foreach (var observer in pillarConfig.PillarObservers)
                    {
                        pillar.OnPillarActivate(observer);
                    }
                }
                pillarCurrentState = PillarStates.Active;
            }
            else if(CanBeEmpowered(starResourceBehavior.GetCurrentResource()))
            {
                starResourceBehavior.DecreaseResource(pillarConfig.EmpowerCost);
                foreach (var pillar in pillarObserverModifiers)
                {
                    foreach (var observer in pillarConfig.PillarObservers)
                    {
                        pillar.SetPillarEmpowerState(observer, true);
                        CronoScheduler.Instance.ScheduleForTime(pillarConfig.EmpoweredDuration, () => 
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
            return currentResources >= pillarConfig.EmpowerCost && pillarCurrentState == PillarStates.Active;
        }

        private bool CanBeActivated(float currentResources)
        {
            return currentResources >= pillarConfig.ActivateCost && pillarCurrentState == PillarStates.Inactive;
        }
    }
}