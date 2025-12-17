using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendActionVisual : MonoBehaviour
{
    [SerializeField] private GameObject defendBuffVisual;

    private void Start()
    {
        GetComponent<DefendAction>().OnDefendStateChanged += DefendActionVisual_OnDefendStateChanged;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void TurnSystem_OnTurnChanged(object sender, System.EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        Hide();
    }

    private void DefendActionVisual_OnDefendStateChanged(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        defendBuffVisual.SetActive(true);
    }

    private void Hide()
    {
        defendBuffVisual.SetActive(false);
    }
}
