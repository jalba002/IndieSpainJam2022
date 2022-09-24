using System;
using System.Collections.Generic;
using CosmosDefender;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    [SerializeField] private PlayerAttributes playerAttributes;
    private AttributesData attackData;
    public Animator animator;
    public Transform FirePoint;

    [SerializeField] private SkillPreview skillPreviewPrefab;

    private SkillPreview skillPreviewer;

    private BaseSpell previewedSpell;

    [SerializeField] private LayerMask previewLayerMask;

    // After casting a spell, add it to the list and start a crono with that.
    private List<BaseSpell> cooldownSpells = new List<BaseSpell>();

    private float timeUntilAvailableCast = 0f;

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

        if (cooldownSpells.Contains(selectedSpell)) return;

        // Cooldown between animation casts.
        if (timeUntilAvailableCast > Time.time) return;

        switch (selectedSpell.castType)
        {
            case CastType.Direct:
                CastSpell(selectedSpell, transform.position);
                skillPreviewer.Deactivate();
                break;
            case CastType.Preview:
                if (previewedSpell == null || selectedSpell != previewedSpell)
                {
                    previewedSpell = selectedSpell;
                    skillPreviewer.Activate();
                    skillPreviewer.UpdateVisuals(previewedSpell.spellData.UniformSize +
                                                 previewedSpell.spellData.ProjectileRadius * 0.5f);
                }
                else if (skillPreviewer.IsActive)
                {
                    CastSpell(previewedSpell, skillPreviewer.transform.position);
                    skillPreviewer.Deactivate();
                    previewedSpell = null;
                }

                break;
            case CastType.Raycast:
                Ray cameraRay = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));

                Physics.Raycast(
                    cameraRay,
                    out RaycastHit info,
                    selectedSpell.spellData.MaxAttackDistance +
                    Vector3.Distance(Camera.main.transform.position, transform.position));

                CastSpell(selectedSpell, info.point);
                skillPreviewer.Deactivate();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void CastPreviewedSpell(ref BaseSpell spell)
    {
        CastSpell(spell, skillPreviewer.transform.position);
        spell = null;
        skillPreviewer.Deactivate();
    }


    void CastSpell(BaseSpell spell, Vector3 pos)
    {
        spell.Cast(pos, transform.forward, Quaternion.identity, this);
        // Add a delay when casting
        timeUntilAvailableCast = Time.time + (spell.spellData.AnimationDelay * 1.5f);

        cooldownSpells.Add(spell);
        CronoScheduler.Instance.ScheduleForTime(spell.spellData.Cooldown, () => { cooldownSpells.Remove(spell); });
    }

    private void Update()
    {
        if (!skillPreviewer.IsActive) return;

        Ray cameraRay = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        if (Physics.Raycast(cameraRay, out RaycastHit hit,
            previewedSpell.spellData.MaxAttackDistance +
            Vector3.Distance(this.transform.position, Camera.main.transform.position), previewLayerMask))
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