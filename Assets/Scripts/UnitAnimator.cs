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

    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;
    [SerializeField] private Transform rifleTransform;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }

        if (TryGetComponent<SwordAction>(out SwordAction swordAction))
        {
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
        }

        if (TryGetComponent<DefendAction>(out DefendAction defendAction))
        {
            defendAction.OnDefendStarted += DefendAction_OnDefendStarted;
        }

        if (TryGetComponent<InteractAction>(out InteractAction interactAction))
        {
            interactAction.OnInteractStarted += InteractAction_OnInteractStarted;            
        }

        if (TryGetComponent<HealAction>(out HealAction healAction))
        {
            healAction.OnHealStarted += HealAction_OnHealStarted;
        }

        if (TryGetComponent<GrenadeAction>(out GrenadeAction grenadeAction))
        {
            grenadeAction.OnThrowStarted += GrenadeAction_OnThrowStarted;
        }


    }

    private void SwordAction_OnSwordActionStarted(object sender, EventArgs e)
    {
        animator.SetTrigger(UNIT_SWORD_SLASH);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool(UNIT_ISWALKING, false);
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool(UNIT_ISWALKING, true);
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger(UNIT_SHOOT);

        Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();

        targetUnitShootAtPosition.y = shootPointTransform.position.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);
    }

    private void DefendAction_OnDefendStarted(object sender, EventArgs e)
    {
        animator.SetTrigger(UNIT_DEFEND);
    }

    private void InteractAction_OnInteractStarted(object sender, EventArgs e)
    {
        animator.SetTrigger(UNIT_INTERACT);
    }

    private void HealAction_OnHealStarted(object sender, EventArgs e)
    {
        animator.SetTrigger(UNIT_HEAL);
    }

    private void GrenadeAction_OnThrowStarted(object sender, EventArgs e)
    {
        animator.SetTrigger(UNIT_THROW);
    }


}
