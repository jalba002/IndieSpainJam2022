using System.Collections;
using System.Collections.Generic;
using CosmosDefender;
using UnityEngine;

public class SpellTester : MonoBehaviour
{
    [SerializeField] private IReadOnlyOffensiveData attackData;

    [SerializeField] private List<BaseSpell> spellList = new List<BaseSpell>();

    void CastSpell(int integer)
    {
        spellList[integer]?.Cast(this.transform, Vector3.forward, Quaternion.identity, attackData);
    }

    void OnSpell1()
    {
        // Maybe a better way?
        CastSpell(0);
    }

    void OnSpell2()
    {
        CastSpell(1);
    }

    void OnSpell3()
    {
        CastSpell(2);
    }

    void OnSpell4()
    {
        CastSpell(3);
    }

    void OnSpell5()
    {
        CastSpell(4);
    }

    void OnSpell6()
    {
        CastSpell(5);
    }

    void OnSpell7()
    {
        CastSpell(6);
    }

    void OnSpell8()
    {
        CastSpell(7);
    }

    void OnSpell9()
    {
        CastSpell(8);
    }
}