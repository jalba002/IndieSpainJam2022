using CosmosDefender;
using System.Collections.Generic;
using UnityEngine;

public class PillarObserver : MonoBehaviour
{
    [SerializeField]
    private List<PillarController> pillarsInRange = new List<PillarController>();

    [SerializeField]
    private PlayerAttributes playerAttributes;

    [SerializeField]
    private PillarsConfig pillarConfig;

    private void Start()
    {
        pillarConfig.AddPillarObserver(this);
    }

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

    public void AddModifiers(List<BaseAttributeModifier> newModifiers)
    {
        foreach (var item in newModifiers)
        {
            playerAttributes.AddAttribute(item);
        }
    }

    public void RemoveModifiers(List<BaseAttributeModifier> modifiers)
    {
        foreach (var item in modifiers)
        {
            playerAttributes.RemoveAttribute(item);
        }
    }
}