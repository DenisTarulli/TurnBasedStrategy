using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private int actionPointsMax = 3;
    [SerializeField] private int maxEnergy = 10;
    [SerializeField] private int expToGive = 0;
    [SerializeField] private int passiveEnergyGain;
    private int currentEnergy;
    private int initialMaxEnergy;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyEnergyChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    public static void ResetStaticData()
    {
        OnAnyActionPointsChanged = null;
        OnAnyEnergyChanged = null;
        OnAnyUnitSpawned = null;
        OnAnyUnitDead = null;
    }

    public event EventHandler OnMaxEnergyChanged;

    [SerializeField] private bool isEnemy;

    private GridPosition gridPosition;
    private HealthSystem healthSystem;
    private BaseAction[] baseActionArray;
    private int currentActionPoints;

    private int spareActionPoints;
    private int spareEnergy;

    private bool hasStolen;
    private bool isDefending;

    private void Awake()
    {
        baseActionArray = GetComponents<BaseAction>();
        healthSystem = GetComponent<HealthSystem>();

        currentActionPoints = actionPointsMax;
        currentEnergy = maxEnergy;
        initialMaxEnergy = maxEnergy;

        hasStolen = true;
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);

        if (!isEnemy)
        {
            PlayerStats.Instance.OnEnergyChanged += PlayerStats_OnEnergyChanged;
        }
    }

    private void PlayerStats_OnEnergyChanged(object sender, EventArgs e)
    {
        maxEnergy = initialMaxEnergy + PlayerStats.Instance.GetEnergy();
        OnMaxEnergyChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if (newGridPosition != gridPosition)
        {
            // Unit changed GridPosition
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;

            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in baseActionArray)
        {
            if (baseAction is T)
            {
                return (T)baseAction;
            }
        }

        return null;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPointsAndEnergyToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction) && CanSpendEnergyToTakeAction(baseAction))
        {
            SpendActionPoint(baseAction.GetActionPointsCost());
            SpendEnergy(baseAction.GetEnergyCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (currentActionPoints >= baseAction.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendEnergyToTakeAction(BaseAction baseAction)
    {
        if (currentEnergy >= baseAction.GetEnergyCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpendActionPoint(int amount)
    {
        currentActionPoints -= amount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SpendEnergy(int amount)
    {
        currentEnergy -= amount;

        OnAnyEnergyChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionsPoints()
    {
        return currentActionPoints;
    }

    public int GetEnergy()
    {
        return currentEnergy;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if ((!IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()))
        {
            spareActionPoints = currentActionPoints;
            spareEnergy = currentEnergy;
        }

        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()))
        {
            currentActionPoints = actionPointsMax;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
        else if (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn())
        {
            int energyToGain = GetNextTurnEnergyRegen();

            currentEnergy += energyToGain;

            currentActionPoints = actionPointsMax; 

            if (currentEnergy > maxEnergy)
            {
                currentEnergy = maxEnergy;
            }

            BuffSystem.Instance.SetEnergyBuff(false);

            SetIsDefending(false);

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
            OnAnyEnergyChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public int GetNextTurnEnergyRegen()
    {
        int energyGain = 0;

        if (BuffSystem.Instance.IsEnergyBuffActive())
        {
            energyGain = 3;
        }

        spareEnergy = currentEnergy;
        spareActionPoints = currentActionPoints;

        energyGain += spareEnergy + spareActionPoints + passiveEnergyGain;

        return energyGain;
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);

        if (IsEnemy())
        {
            PlayerStats.Instance.ChangeExp(expToGive);
            Destroy(gameObject);
        }
        else
        {
            GameManager.Instance.GameOver();
        }
        
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }

    public bool HasStolen()
    {
        return hasStolen;
    }

    public void ToggleHasStolen()
    {
        hasStolen = !hasStolen;
    }

    public void SetIsDefending(bool newState)
    {
        isDefending = newState;
    }

    public bool IsDefending()
    {
        return isDefending;
    }

    public int GetMaxEnergy()
    {
        return maxEnergy;
    }

    public int GetMaxActionPoints()
    {
        return actionPointsMax;
    }

    public int GetActionPoints()
    {
        return currentActionPoints;
    }
}
