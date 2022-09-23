using System;
using CosmosDefender;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;

public class SpellManager : MonoBehaviour
{
    [SerializeField] private PlayerAttributes playerAttributes;
    private AttributesData attackData;
    public Animator animator;
    public Transform FirePoint;

    [SerializeField] private SkillPreview skillPreviewPrefab;

    private SkillPreview skillPreviewer;

    private BaseSpell previewedSpell;

    [SerializeField] private LayerMask layerMask;

    private void Awake()
    {
        playerAttributes.Initialize();

        animator = GetComponentInChildren<Animator>();

        if (skillPreviewer == null)
        {
            skillPreviewer = Instantiate(skillPreviewPrefab, transform.position, Quaternion.identity);
        }
    }

    private void Start()
    {
        skillPreviewer.Deactivate();
    }

    void CastSpell(SpellKeyType spellKey)
    {
        if (!playerAttributes.HasSpellKey(spellKey))
            return;

        var selectedSpell = playerAttributes.GetSpell(spellKey);
        
        if (selectedSpell.spellData.UsesPreview && selectedSpell != previewedSpell)
        {
            previewedSpell = selectedSpell;
            skillPreviewer.Activate();
            skillPreviewer.UpdateVisuals(previewedSpell.spellData.UniformSize + previewedSpell.spellData.ProjectileRadius * 0.5f);
        }
        else if (skillPreviewer.IsActive && selectedSpell == previewedSpell)
        {
            CastPreviewedSpell(ref previewedSpell);
        }
        else
        {
            CastSpell(selectedSpell);
            previewedSpell = null;
        }
    }

    void CastPreviewedSpell(ref BaseSpell spell)
    {
        spell.Cast(skillPreviewer.transform.position, transform.forward, Quaternion.identity, this);
        spell = null;
        skillPreviewer.Deactivate();
    }

    void CastSpell(BaseSpell spell)
    {
        spell.Cast(transform.position, transform.forward, Quaternion.identity, this);
        skillPreviewer.Deactivate();
    }

    private void Update()
    {
        if (!skillPreviewer.IsActive) return;

        Ray cameraRay = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        if (Physics.Raycast(cameraRay, out RaycastHit hit,
            previewedSpell.spellData.MaxAttackDistance +
            Vector3.Distance(this.transform.position, Camera.main.transform.position), layerMask))
            skillPreviewer.Move(hit.point);
    }

    void OnFire()
    {
        if (skillPreviewer.IsActive)
        {
            CastPreviewedSpell(ref previewedSpell);
        }
        else
        {
            CastSpell(SpellKeyType.Spell0);
        }
    }

    void OnAltFire()
    {
        // Cancel preview.
        if (previewedSpell == null) return;
        skillPreviewer.Deactivate();
        previewedSpell = null;
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