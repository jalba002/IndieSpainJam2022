using CosmosDefender;
using UnityEngine;

public class SpellTester : MonoBehaviour
{
    [SerializeField] private PlayerAttributes playerAttributes;
    [SerializeField] private AttributesData attackData;
    public Animator animator;
    public Transform FirePoint;

    private void Awake()
    {
        playerAttributes.Initialize();

        animator = GetComponentInChildren<Animator>();
    }

    void CastSpell(SpellKeyType spellKey)
    {
        if (!playerAttributes.HasSpellKey(spellKey))
            return;

        var selectedSpell = playerAttributes.GetSpell(spellKey);
        var ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit raycastHit, selectedSpell.spellData.MaxAttackDistance))
        {
            selectedSpell.Cast(raycastHit.point, ray.direction, Quaternion.identity, this);
        }
    }

    void OnSpell1()
    {
        // Maybe a better way?
        CastSpell(SpellKeyType.Spell0);
        animator.SetTrigger("CastMeteor");
    }

    void OnSpell2()
    {
        CastSpell(SpellKeyType.Spell1);
        animator.SetTrigger("CastLaser");
    }

    void OnSpell3()
    {
        CastSpell(SpellKeyType.Spell4);
    }

    void OnSpell4()
    {
        //CastSpell(SpellKeyType.Spell4);
    }

    void OnSpell5()
    {
        //CastSpell(4);
    }

    void OnSpell6()
    {
        //CastSpell(5);
    }

    void OnSpell7()
    {
        //CastSpell(6);
    }

    void OnSpell8()
    {
        //CastSpell(7);
    }

    void OnSpell9()
    {
        //CastSpell(8);
    }
}