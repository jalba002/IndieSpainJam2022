using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(PillarsConfig), menuName = "CosmosDefender/" + nameof(PillarsConfig))]
public class PillarsConfig : ScriptableObject
{
    public List<PillarObserver> PillarObservers = new List<PillarObserver>();
    public float Range = 5f;

    public void ClearObserverList()
    {
        PillarObservers.Clear();
    }

    public void AddPillarObserver(PillarObserver newPillarObserver)
    {
        PillarObservers.Add(newPillarObserver);
    }

    public void AddPillarObservers(List<PillarObserver> newPillarObservers)
    {
        foreach (var observer in newPillarObservers)
        {
            PillarObservers.Add(observer);
        }
    }
}