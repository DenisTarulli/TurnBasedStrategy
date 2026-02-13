using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private const string IS_OPEN = "IsOpen";

    [SerializeField] private int goldDrop = 5;

    private Animator animator;
    private GridPosition gridPosition;
    private Action onInteractionComplete;
    private bool isActive;
    private float timer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        timer -= Time.deltaTime;
        animator.SetTrigger(IS_OPEN);

        if (timer <= 0f)
        {
            isActive = false;
            LevelGrid.Instance.ClearInteractableAtGridPosition(gridPosition);

            InventoryManager.Instance.ChangeKeysAmount(-1);
            InventoryManager.Instance.ChangeGoldAmount(goldDrop);

            onInteractionComplete();
        }
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        float interactDuration = 0.5f;
        timer = interactDuration;
        isActive = true;
    }
    public SoundManager.SoundType GetInteractSound()
    {
        return SoundManager.SoundType.AbrirCofre;
    }
}
