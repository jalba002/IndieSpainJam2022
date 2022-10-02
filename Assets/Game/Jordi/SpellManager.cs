using System;
using System.Collections.Generic;
using System.Linq;
using CosmosDefender;
using FMOD;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

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
    [SerializeField] private LayerMask damagingCastLayerMask;

    [Header("Components")] [SerializeField]
    private ResourceManager resourceManager;

    // After casting a spell, add it to the list and start a crono with that.
    private List<ISpell> _cooldownSpells = new List<ISpell>();

    private float timeUntilAvailableCast = 0f;

    public Action<ISpell, float> OnSpellCasted;

    private bool holdingSpell = false;
    private BaseSpell heldSpell;

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
        if (holdingSpell) return;
        
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
                var camTransform = Camera.main.transform;
                float cameraCorrectedDistance = selectedSpell.spellData.MaxAttackDistance +
                                                Vector3.Distance(camTransform.position, transform.position);
                var enemyHits = AreaAttacksManager.BoxAttack(camTransform.position, camTransform.forward,
                    new Vector3(1f, 1f, cameraCorrectedDistance) * 0.5f, Quaternion.identity, damagingCastLayerMask);

                bool rayHit;

                Ray usedRay;
                if (enemyHits.Length > 0)
                {
                    usedRay = new Ray(
                        CastingPoint.position,
                        (enemyHits[Utils.GetClosestIndexFromList(camTransform, enemyHits.ToList())].transform.position -
                         CastingPoint.position).normalized
                    );
                }
                else
                {
                    usedRay = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
                }

                rayHit = Physics.Raycast(
                    usedRay,
                    out RaycastHit info,
                    cameraCorrectedDistance,
                    damagingCastLayerMask
                );


                Vector3 castPos =
                    rayHit ? info.point : usedRay.origin + usedRay.direction * cameraCorrectedDistance;

                CastSpell(selectedSpell, castPos);
                skillPreviewer.Deactivate();
                break;
            case CastType.Held:
                holdingSpell = true;
                heldSpell = selectedSpell;
                //CastSpell(selectedSpell, transform.position);
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

        float cd = spell.spellData.Cooldown -
                   (spell.spellData.Cooldown * playerAttributes.CombatData.CooldownReduction / 100);

        OnSpellCasted?.Invoke(spell, cd);

        if (cd <= 0) return;
        // Avoid adding it to cooldown. Skips a frame.
        timeUntilAvailableCast = Time.time + (spell.spellData.AnimationDelay * 1.5f);

        _cooldownSpells.Add(spell);
        CronoScheduler.Instance.ScheduleForTime(
            cd,
            () => { _cooldownSpells.Remove(spell); });
    }

    void StopCastingHeldSpell()
    {
        heldSpell.StopCast();
        heldSpell = null;
    }

    private void CastHeldSpell()
    {
        CastSpell(heldSpell, transform.position);
    }

    private void Update()
    {
        if(holdingSpell)
            CastHeldSpell();
        
        if (!skillPreviewer.IsActive) return;

        Ray cameraRay = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        float cameraCorrectedDistance = previewedSpell.spellData.MaxAttackDistance +
                                        Vector3.Distance(this.transform.position, Camera.main.transform.position);

        bool firstRayhit = Physics.Raycast(cameraRay, out RaycastHit hit, cameraCorrectedDistance, previewLayerMask);

        Vector3 castPos = firstRayhit ? hit.point : cameraRay.origin + cameraRay.direction * cameraCorrectedDistance;

        if (!firstRayhit)
        {
            Ray verticalRay = new Ray(castPos, Vector3.down);
            if (Physics.Raycast(verticalRay, out RaycastHit info2, cameraCorrectedDistance, previewLayerMask))
            {
                castPos = info2.point;
            }
        }

        skillPreviewer.Move(castPos);
    }

    void OnFire(InputValue value)
    {
        if (skillPreviewer.IsActive)
        {
            CastPreviewedSpell(ref previewedSpell);
        }
        else
        {
            float val = value.Get<float>();
            Debug.Log(val);
            if (val > 0f && !holdingSpell)
            {
                CastSpell(SpellKeyType.Spell0);
            }
            else
            {
                StopCastingHeldSpell();
                holdingSpell = false;
            }
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

    void OnGoddessMode()
    {
        var selectedSpell = playerAttributes.GetUltimate();

        if (_cooldownSpells.Contains(selectedSpell)) return;

        if (timeUntilAvailableCast > Time.time) return;

        if (resourceManager.SpendResource(ResourceType.Goddess,
            resourceManager.GetResourceData(ResourceType.Goddess).MaxResource))
        {
            //GameManager.Instance.ActivateGoddessMode();
            // Tell the player the Goddess Mode has been activated.
            //animator.SetTrigger("GoddessMode");
            CastUltimate(selectedSpell);
        }
    }

    private void CastUltimate(BaseSpell selectedSpell)
    {
        switch (selectedSpell.castType)
        {
            case CastType.Direct:
                CastSpell(selectedSpell, transform.position);
                skillPreviewer.Deactivate();
                previewedSpell = null;
                //Cast(transform.position, transform.forward, Quaternion.identity, playerAttributes.CombatData, this);
                break;
            case CastType.Preview:
                Debug.LogWarning("Can't cast a previewable Ultimate yet.");
                break;
            case CastType.Raycast:
                Debug.LogWarning("Can't cast a raycastable Ultimate yet.");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public GameObject GameObject => this.gameObject;
    public Animator Animator => animator;
    public Transform CastingPoint => FirePoint;
    public void SetAnimationTrigger(string triggerName) => Animator.SetTrigger(triggerName);
}