using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpUtils : MonoBehaviour
{
    private bool isActive;
    private bool isBusy;

    private void Start()
    {
        PlayerStats.Instance.OnLevelUp += PlayerStats_OnLevelUp;
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
    }

    private void UnitActionSystem_OnBusyChanged(object sender, bool e)
    {
        isBusy = UnitActionSystem.Instance.IsBusy();
    }

    private void PlayerStats_OnLevelUp(object sender, System.EventArgs e)
    {
        ToggleIsActive();
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        if (isBusy)
        {
            return;
        }

        ShopSystemUI.Instance.Show();
        ToggleIsActive();
    }

    private void ToggleIsActive()
    {
        isActive = !isActive;
    }
}
