using System;
using UnityEngine;

public class UnitSound : MonoBehaviour
{
    [Header("Clips de Sonido")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip swordClip;
    [SerializeField] private AudioClip grenadeClip;
    [SerializeField] private AudioClip healClip;
    [SerializeField] private AudioClip defendClip;
    [SerializeField] private AudioClip interactClip;
    [SerializeField] private AudioClip damageClip;

    [Header("Pasos")]
    [SerializeField] private AudioClip[] footstepClips;

    // Guardamos referencias para la desuscripción limpia
    private MoveAction moveAction;
    private ShootAction shootAction;
    private SwordAction swordAction;
    private GrenadeAction grenadeAction;
    private HealAction healAction;
    private DefendAction defendAction;
    private InteractAction interactAction;
    private HealthSystem healthSystem;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out moveAction))
        {
            // Nota: Si tus pasos los vas a disparar por Animation Events, no hace falta el evento OnStartMoving.
        }

        if (TryGetComponent<ShootAction>(out shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }

        if (TryGetComponent<SwordAction>(out swordAction))
        {
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
        }

        if (TryGetComponent<GrenadeAction>(out grenadeAction))
        {
            grenadeAction.OnGrenadeActionStarted += GrenadeAction_OnGrenadeActionStarted;
        }

        if (TryGetComponent<HealAction>(out healAction))
        {
            healAction.OnHealActionStarted += HealAction_OnHealActionStarted;
        }

        if (TryGetComponent<DefendAction>(out defendAction))
        {
            defendAction.OnDefendActionStarted += DefendAction_OnDefendActionStarted;
        }

        if (TryGetComponent<InteractAction>(out interactAction))
        {
            interactAction.OnInteractActionStarted += InteractAction_OnInteractActionStarted;
        }

        if (TryGetComponent<HealthSystem>(out healthSystem))
        {
            healthSystem.OnDamaged += HealthSystem_OnDamaged;
        }
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        SoundManager.Instance.PlaySound(shootClip, transform.position);
    }

    private void SwordAction_OnSwordActionStarted(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(swordClip, transform.position);
    }

    private void GrenadeAction_OnGrenadeActionStarted(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(grenadeClip, transform.position);
    }

    private void HealAction_OnHealActionStarted(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(healClip, transform.position);
    }

    private void DefendAction_OnDefendActionStarted(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(defendClip, transform.position);
    }

    private void InteractAction_OnInteractActionStarted(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(interactClip, transform.position);
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(damageClip, transform.position);
    }

    // Método público para ser invocado desde los Animation Events al caminar
    public void PlayFootstepSound()
    {
        SoundManager.Instance.PlayRandomSound(footstepClips, transform.position);
    }

    private void OnDestroy()
    {
        // Desuscripción obligatoria para prevenir Memory Leaks cuando destruyas unidades
        if (shootAction != null) shootAction.OnShoot -= ShootAction_OnShoot;
        if (swordAction != null) swordAction.OnSwordActionStarted -= SwordAction_OnSwordActionStarted;
        if (grenadeAction != null) grenadeAction.OnGrenadeActionStarted -= GrenadeAction_OnGrenadeActionStarted;
        if (healAction != null) healAction.OnHealActionStarted -= HealAction_OnHealActionStarted;
        if (defendAction != null) defendAction.OnDefendActionStarted -= DefendAction_OnDefendActionStarted;
        if (interactAction != null) interactAction.OnInteractActionStarted -= InteractAction_OnInteractActionStarted;
        if (healthSystem != null) healthSystem.OnDamaged -= HealthSystem_OnDamaged;
    }
}