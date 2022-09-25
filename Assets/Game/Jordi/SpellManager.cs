using System;
using System.Collections.Generic;
using CosmosDefender;
using Unity.VisualScripting;
using UnityEngine;

public class SpellManager : MonoBehaviour, ISpellCaster
{
    [SerializeField] private PlayerAttributes playerAttributes;
    private AttributesData attackData;
    [SerializeField] private Animator animator;
    public Transform FirePoint;

    [SerializeField] private SkillPreview skillPreviewPrefab;

    private SkillPreview skillPreviewer;

    private ISpell previewedSpell;

    [SerializeField] private LayerMask previewLayerMask;

    // After casting a spell, add it to the list and start a crono with that.
    private readonly List<ISpell> _cooldownSpells = new List<ISpell>();

    private float timeUntilAvailableCast = 0f;

    public Action<ISpell> OnSpellCasted;

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

        if (_cooldownSpells.Contains(selectedSpell)) return;

        // Cooldown between animation casts.
        if (timeUntilAvailableCast > Time.time) return;

        switch (selectedSpell.castType)
        {
            case CastType.Direct:
                CastSpell(selectedSpell, transform.position);
                skillPreviewer.Deactivate();
                previewedSpell = null;
                break;
            case CastType.Preview:
                if (previewedSpell == null)
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
                float cameraCorrectedDistance = selectedSpell.spellData.MaxAttackDistance +
                                                Vector3.Distance(Camera.main.transform.position, transform.position);
                bool rayHit = Physics.Raycast(
                    cameraRay,
                    out RaycastHit info,
                    cameraCorrectedDistance
                    );

                Vector3 castPos =
                    rayHit ? info.point : cameraRay.origin + cameraRay.direction * cameraCorrectedDistance;
                
                CastSpell(selectedSpell, castPos);
                skillPreviewer.Deactivate();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void CastPreviewedSpell(ref ISpell spell)
    {
        CastSpell(spell, skillPreviewer.transform.position);
        spell = null;
        skillPreviewer.Deactivate();
    }


    void CastSpell(ISpell spell, Vector3 pos)
    {
        spell.Cast(pos, transform.forward, Quaternion.identity, playerAttributes.CombatData, this);
        // Add a delay when casting
        timeUntilAvailableCast = Time.time + (spell.spellData.AnimationDelay * 1.5f);

        OnSpellCasted.Invoke(spell);
        
        _cooldownSpells.Add(spell);
        CronoScheduler.Instance.ScheduleForTime(
            spell.spellData.Cooldown - (spell.spellData.Cooldown * playerAttributes.CombatData.CooldownReduction / 100), 
            () => { _cooldownSpells.Remove(spell); });
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

    public GameObject GameObject => this.gameObject;
    public Animator Animator => animator;
    public Transform CastingPoint => FirePoint;
    public void SetAnimationTrigger(string triggerName) => Animator.SetTrigger(triggerName);

}