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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, pillarConfig.Range);
            Gizmos.color = Color.white;
        }

        private void Awake()
        {
            GetComponents(pillarObserverModifiers);
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
                        Debug.Log("Inside range");
                        observersInRange.Add(observer);
                    }
                }
            }
        }

        [Button]
        public void ActivatePillar()
        {
            foreach (var pillar in pillarObserverModifiers)
            {
                foreach (var observer in observersInRange)
                {
                    pillar.OnPillarActivate(observer);
                }
            }
        }
    }
}