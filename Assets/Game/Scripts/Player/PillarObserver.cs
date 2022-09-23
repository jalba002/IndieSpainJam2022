using System.Collections.Generic;
using CosmosDefender;
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

    public bool IsPillarInRange()
    {
        return pillarsInRange.Count > 0;
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
        playerAttributes.AddAttributeModifier(newModifier);
    }

    public void RemoveModifier(BaseAttributeModifier modifier)
    {
        playerAttributes.RemoveAttributeModifier(modifier);
    }

    public void AddModifiers(List<BaseAttributeModifier> newModifiers)
    {
        foreach (var item in newModifiers)
        {
            playerAttributes.AddAttributeModifier(item);
        }
    }

    public void RemoveModifiers(List<BaseAttributeModifier> modifiers)
    {
        foreach (var item in modifiers)
        {
            playerAttributes.RemoveAttributeModifier(item);
        }
    }

    public void AddSpell(CosmosSpell newSpell)
    {
        playerAttributes.AddSpell(newSpell);
    }

    public void SetSpellEmpowerState(CosmosSpell newSpell, bool newState)
    {
        newSpell.isSpellEmpowered = newState;
    }
}