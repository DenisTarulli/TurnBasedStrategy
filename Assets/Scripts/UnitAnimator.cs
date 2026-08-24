using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private const string UNIT_ISWALKING = "IsWalking";
    private const string UNIT_SHOOT = "Shoot";
    private const string UNIT_SWORD_SLASH = "SwordSlash";
    private const string UNIT_GRENADE_THROW = "GrenadeThrow";
    private const string UNIT_HEAL = "Heal";
    private const string UNIT_DEFEND = "Defend";
    private const string UNIT_INTERACT = "Interact";
    private const string UNIT_HIT = "Hit";

    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;
    [SerializeField] private Transform rifleTransform;
    [SerializeField] private Transform swordTransform;

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
            swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted;
        }

        if (TryGetComponent<GrenadeAction>(out GrenadeAction grenadeAction))
        {
            grenadeAction.OnGrenadeActionStarted += GrenadeAction_OnGrenadeActionStarted;
        }

        if (TryGetComponent<HealAction>(out HealAction healAction))
        {
            healAction.OnHealActionStarted += HealAction_OnHealActionStarted;
        }

        if (TryGetComponent<DefendAction>(out DefendAction defendAction))
        {
            defendAction.OnDefendActionStarted += DefendAction_OnDefendActionStarted;
        }

        if (TryGetComponent<InteractAction>(out InteractAction interactAction))
        {
            interactAction.OnInteractActionStarted += InteractAction_OnInteractActionStarted;
        }

        if (TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
        {
            healthSystem.OnDamaged += HealthSystem_OnDamaged;
        }
    }

    private void Start()
    {
        EquipRifle();
    }

    private void SwordAction_OnSwordActionCompleted(object sender, EventArgs e)
    {
        EquipRifle();
    }

    private void SwordAction_OnSwordActionStarted(object sender, EventArgs e)
    {
        EquipSword();
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

    private void GrenadeAction_OnGrenadeActionStarted(object sender, EventArgs e)
    {
        animator.SetTrigger(UNIT_GRENADE_THROW);
    }

    private void HealAction_OnHealActionStarted(object sender, EventArgs e)
    {
        animator.SetTrigger(UNIT_HEAL);
    }

    private void DefendAction_OnDefendActionStarted(object sender, EventArgs e)
    {
        animator.SetTrigger(UNIT_DEFEND);
    }

    private void InteractAction_OnInteractActionStarted(object sender, EventArgs e)
    {
        animator.SetTrigger(UNIT_INTERACT);
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        animator.SetTrigger(UNIT_HIT);
    }

    private void EquipSword()
    {
        swordTransform.gameObject.SetActive(true);
        rifleTransform.gameObject.SetActive(false);
    }

    private void EquipRifle()
    {
        swordTransform.gameObject.SetActive(false);
        rifleTransform.gameObject.SetActive(true);
    }
}