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
    private List<ISpell> _cooldownSpells = new List<ISpell>();

    private float timeUntilAvailableCast = 0f;

    public Action<ISpell, float> OnSpellCasted;

    private void Awake()
    {
        playerAttributes.Initialize();

        animator = GetComponentInChildren<Animator>();

        if (skillPreviewer == null)
        {
            skillPreviewer = Instantiate(skillPreviewPrefab, transform.position, Quaternion.identity);
        }

        GetComponent<GoddessResourceBehavior>().OnActivation += () => { _cooldownSpells = new List<ISpell>(); };
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
                    cameraCorrectedDistance,
                    previewLayerMask
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

        float cd = spell.spellData.Cooldown -
                   (spell.spellData.Cooldown * playerAttributes.CombatData.CooldownReduction / 100);
        OnSpellCasted?.Invoke(spell, cd);

        _cooldownSpells.Add(spell);
        CronoScheduler.Instance.ScheduleForTime(
            cd,
            () => { _cooldownSpells.Remove(spell); });
    }

    private void Update()
    {
        if (!skillPreviewer.IsActive) return;

        Ray cameraRay = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        float cameraCorrectedDistance = previewedSpell.spellData.MaxAttackDistance +
                                        Vector3.Distance(this.transform.position, Camera.main.transform.position);

        bool firstRayhit = Physics.Raycast(cameraRay, out RaycastHit hit, cameraCorrectedDistance, previewLayerMask);

        Vector3 castPos = firstRayhit ? hit.point : cameraRay.origin + cameraRay.direction * cameraCorrectedDistance;

        if(!firstRayhit)
        {
            Ray verticalRay = new Ray(castPos, Vector3.down);
            if (Physics.Raycast(verticalRay, out RaycastHit info2, cameraCorrectedDistance, previewLayerMask))
            {
                castPos = info2.point;
            }
        }

        skillPreviewer.Move(castPos);
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