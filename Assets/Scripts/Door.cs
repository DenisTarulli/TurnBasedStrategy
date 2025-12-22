using System;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public static event EventHandler OnAnyDoorOpened;

    public static void ResetStaticData()
    {
        OnAnyDoorOpened = null;
    }

    private const string IS_OPEN = "IsOpen";

    [SerializeField] private bool isOpen;

    private GridPosition gridPosition;
    private Animator animator;
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

        if (timer <= 0f)
        {
            isActive = false;
            onInteractionComplete();
        }
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        float animDuration = 1f;
        timer = animDuration;

        OpenDoor();

        OnAnyDoorOpened?.Invoke(this, EventArgs.Empty);
    }

    private void OpenDoor()
    {
        isOpen = true;
        animator.SetBool(IS_OPEN, isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
    }
    
    public bool IsOpen()
    {
        return isOpen;
    }
}
