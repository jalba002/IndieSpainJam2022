using CosmosDefender;
using System.Collections.Generic;
using UnityEngine;

public class PillarObserver : MonoBehaviour
{
    [SerializeField]
    private List<PillarController> pillarsInRange = new List<PillarController>();

    [SerializeField]
    private PlayerAttributes playerAttributes;

    public void AddPillar(PillarController newPillar)
    {
        pillarsInRange.Add(newPillar);
    }

    public void RemovePillar(PillarController pillar)
    {
        pillarsInRange.Remove(pillar);
    }

    public void AddModifier(BaseAttributeModifier newModifier)
    {
        playerAttributes.AddAttribute(newModifier);
    }

    public void RemoveModifier(BaseAttributeModifier modifier)
    {
        playerAttributes.RemoveAttribute(modifier);
    }
}