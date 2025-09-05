using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;

    public void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

        HideActionCamera();
    }

    private void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }
    private void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                // Shoulder height
                Vector3 cameraCharacterHeight = Vector3.up * 1.7f;

                // Direction from the shooter to the target unit
                Vector3 shootDirection = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

                // Horizontal offset to match the shoulder
                float shoulderOffsetAmount = 0.5f;
                Vector3 shoulderOffset = Quaternion.Euler(0f, 90f, 0f) * shootDirection * shoulderOffsetAmount;

                // Feet position
                // Up (height)
                // Right (shoulder)
                // Push back
                Vector3 actionCameraPosition = 
                    shooterUnit.GetWorldPosition() + 
                    cameraCharacterHeight + 
                    shoulderOffset + 
                    (shootDirection * -1);

                // Set the position
                actionCameraGameObject.transform.position = actionCameraPosition;
                // Set the rotation (looking at the target unit position plus its height)
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);
                ShowActionCamera();
                break;
        }
    }
    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }
}
