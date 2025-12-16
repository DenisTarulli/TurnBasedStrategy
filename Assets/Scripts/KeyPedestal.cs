using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPedestal : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject keyObjectVisual;
    private GridPosition gridPosition;
    private Action onInteractionComplete;
    private bool isActive;
    private float timer;
    private bool hasKeyToCollect;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        hasKeyToCollect = true;
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
            InventoryManager.Instance.SetHasKey(true);

            LevelGrid.Instance.ClearInteractableAtGridPosition(gridPosition);

            Debug.Log("Collected key");
            HideKeyVisual();
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

    public bool HasKeyToCollect()
    {
        return hasKeyToCollect;
    }

    private void HideKeyVisual()
    {
        keyObjectVisual.SetActive(false);
    }
}
