using CosmosDefender;
using UnityEngine;
using UnityEngine.ProBuilder;

public class SpellManager : MonoBehaviour
{
    [SerializeField] private PlayerAttributes playerAttributes;
    private AttributesData attackData;
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
        // Maybe this should be the previewable spells.
        // Maybe here is the castType?
        if (selectedSpell.spellData.SpellType == SpellType.Meteor)
        {
            var ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            if (Physics.Raycast(ray, out RaycastHit raycastHit, selectedSpell.spellData.MaxAttackDistance))
            {
                selectedSpell.Cast(raycastHit.point, ray.direction, Quaternion.identity, this);
            }
        }
        else
        {
            selectedSpell.Cast(transform.position, transform.forward, Quaternion.identity, this);
        }

        //}
    }

    void OnFire()
    {
        CastSpell(SpellKeyType.Spell0);
    }

    void OnSpell1()
    {
        // Maybe a better way?
        CastSpell(SpellKeyType.Spell1);
    }

    void OnSpell2()
    {
        CastSpell(SpellKeyType.Spell2);
    }

    void OnSpell3()
    {
        CastSpell(SpellKeyType.Spell3);
    }
}