using CosmosDefender;
using System.Collections.Generic;
using UnityEngine;

public class PillarController : MonoBehaviour
{
    [SerializeField]
    private BaseAttributeModifier attributeModifier;
    [SerializeField]
    private List<PillarObserver> pillarObservers = new List<PillarObserver>();
    private List<PillarObserver> observersInRange = new List<PillarObserver>();
    [SerializeField]
    private float range = 5f;


    private void Update()
    {
        foreach (var observer in pillarObservers)
        {
            var distanceFromObserver = Vector3.Distance(transform.position, observer.transform.position);

            if (observersInRange.Contains(observer))
            {
                if (distanceFromObserver > range)
                {
                    observer.RemoveModifier(attributeModifier);
                    observersInRange.Remove(observer);
                }
            }
            else
            {
                if (distanceFromObserver <= range)
                {
                    observer.AddModifier(attributeModifier);
                    observersInRange.Add(observer);
                }
            }
        }
    }

    public void AddPillarObserver(PillarObserver newPillarObserver)
    {
        pillarObservers.Add(newPillarObserver);
    }

    public void AddPillarObservers(List<PillarObserver> newPillarObservers)
    {
        foreach (var observer in newPillarObservers)
        {
            pillarObservers.Add(observer);
        }
    }
}