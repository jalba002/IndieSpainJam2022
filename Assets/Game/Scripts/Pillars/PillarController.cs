using CosmosDefender;
using System.Collections.Generic;
using UnityEngine;

public class PillarController : MonoBehaviour
{
    [SerializeField]
    private List<BaseAttributeModifier> attributeModifier = new List<BaseAttributeModifier>();
    //[SerializeField]
    //private BaseSpellModifier[] attributeModifier;
    private List<PillarObserver> observersInRange = new List<PillarObserver>();
    [SerializeField]
    private PillarsConfig pillarConfig;

    private void Update()
    {
        foreach (var observer in pillarConfig.PillarObservers)
        {
            var distanceFromObserver = Vector3.Distance(transform.position, observer.transform.position);

            if (observersInRange.Contains(observer))
            {
                if (distanceFromObserver > pillarConfig.Range)
                {
                    observer.RemoveModifiers(attributeModifier);
                    observersInRange.Remove(observer);
                }
            }
            else
            {
                if (distanceFromObserver <= pillarConfig.Range)
                {
                    observer.AddModifiers(attributeModifier);
                    observersInRange.Add(observer);
                }
            }
        }
    }
}