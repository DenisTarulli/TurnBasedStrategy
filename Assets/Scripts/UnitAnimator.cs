using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private const string UNIT_ISWALKING = "IsWalking";
    private const string UNIT_SHOOT = "Shoot";
    private const string UNIT_SWORD_SLASH = "SwordSlash";
    private const string UNIT_DEFEND = "Defend";
    private const string UNIT_INTERACT = "Interact";
    private const string UNIT_HEAL = "Heal";
    private const string UNIT_THROW = "Throw";
    private const string UNIT_HIT = "Hit";

    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;
    [SerializeField] private Transform rifleTransform;

    private HealthSystem healthSystem;
    private MoveAction moveAction;
    private ShootAction shootAction;
    private SwordAction swordAction;
    private DefendAction defendAction;
    private InteractAction interactAction;
    private HealAction healAction;
    private GrenadeAction grenadeAction;

    private Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();

        animator ??= GetComponent<Animator>();

        moveAction = GetComponent<MoveAction>();
        if (moveAction != null)
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        shootAction = GetComponent<ShootAction>();
        if (shootAction != null)
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }

        swordAction = GetComponent<SwordAction>();
        if (swordAction != null)
        {
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
        }

        defendAction = GetComponent<DefendAction>();
        if (defendAction != null)
        {
            defendAction.OnDefendStarted += DefendAction_OnDefendStarted;
        }

        interactAction = GetComponent<InteractAction>();
        if (interactAction != null)
        {
            interactAction.OnInteractStarted += InteractAction_OnInteractStarted;
        }

        healAction = GetComponent<HealAction>();
        if (healAction != null)
        {
            healAction.OnHealStarted += HealAction_OnHealStarted;
        }

        grenadeAction = GetComponent<GrenadeAction>();
        if (grenadeAction != null)
        {
            grenadeAction.OnThrowStarted += GrenadeAction_OnThrowStarted;
        }

        healthSystem = GetComponent<HealthSystem>();
        if (healthSystem != null)
        {
            healthSystem.OnDamaged += HealthSystem_OnDamaged;
        }
    }


    private void SwordAction_OnSwordActionStarted(object sender, EventArgs e)
    {
        if (unit != null && unit.IsDead()) return;
        animator.SetTrigger(UNIT_SWORD_SLASH);
        SoundManager.Instance.PlaySFX(unit.GetSwordHitSound());
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        if (unit != null && unit.IsDead()) return;
        animator.SetBool(UNIT_ISWALKING, false);
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        if (unit != null && unit.IsDead()) return;
        animator.SetBool(UNIT_ISWALKING, true);
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        if (unit != null && unit.IsDead()) return;
        animator.SetTrigger(UNIT_SHOOT);

        Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();

        targetUnitShootAtPosition.y = shootPointTransform.position.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);
    }

    private void DefendAction_OnDefendStarted(object sender, EventArgs e)
    {
        if (unit != null && unit.IsDead()) return;
        animator.SetTrigger(UNIT_DEFEND);
    }

    private void InteractAction_OnInteractStarted(object sender, EventArgs e)
    {
        if (unit != null && unit.IsDead()) return;

        animator.SetTrigger(UNIT_INTERACT);

        if (sender is InteractAction interactAction)
        {
            IInteractable interactable = interactAction.GetCurrentInteractable();

            if (interactable is IInteractSound soundProvider)
            {
                SoundManager.Instance.PlaySFX(soundProvider.GetInteractSound());
            }
        }
    }

    private void HealAction_OnHealStarted(object sender, EventArgs e)
    {
        if (unit != null && unit.IsDead()) return;
        animator.SetTrigger(UNIT_HEAL);
    }

    private void GrenadeAction_OnThrowStarted(object sender, EventArgs e)
    {
        if (unit != null && unit.IsDead()) return;
        animator.SetTrigger(UNIT_THROW);
        SoundManager.Instance.PlaySFX(SoundManager.SoundType.GranadaExplosion);
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        if (unit != null && unit.IsDead()) return;
        if (animator == null) return;
        animator.SetTrigger(UNIT_HIT);
    }


    private void OnDestroy()
    {
        if (moveAction != null)
        {
            moveAction.OnStartMoving -= MoveAction_OnStartMoving;
            moveAction.OnStopMoving -= MoveAction_OnStopMoving;
        }

        if (shootAction != null)
        {
            shootAction.OnShoot -= ShootAction_OnShoot;
        }

        if (swordAction != null)
        {
            swordAction.OnSwordActionStarted -= SwordAction_OnSwordActionStarted;
        }

        if (defendAction != null)
        {
            defendAction.OnDefendStarted -= DefendAction_OnDefendStarted;
        }

        if (interactAction != null)
        {
            interactAction.OnInteractStarted -= InteractAction_OnInteractStarted;
        }

        if (healAction != null)
        {
            healAction.OnHealStarted -= HealAction_OnHealStarted;
        }

        if (grenadeAction != null)
        {
            grenadeAction.OnThrowStarted -= GrenadeAction_OnThrowStarted;
        }

        if (healthSystem != null)
        {
            healthSystem.OnDamaged -= HealthSystem_OnDamaged;
        }
    }

}
