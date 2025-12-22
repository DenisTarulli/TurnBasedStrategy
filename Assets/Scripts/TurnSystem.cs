using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }    

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one TurnSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        turnLimit = turnLimitArray[currentRoom];
    }

    public event EventHandler OnTurnChanged;
    public event EventHandler OnNewRoomEntered;

    private int turnNumber = 1;
    private int turnLimit;
    private int currentRoom = 0;
    private bool isPlayerTurn = true;
    [SerializeField] private int additionalRooms;

    [SerializeField] private int[] turnLimitArray;
    [SerializeField] private Door[] doorArray;
    [SerializeField] private GameObject[] roomArray;

    private void Start()
    {
        Door.OnAnyDoorOpened += Door_OnAnyDoorOpened;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        if (currentRoom != additionalRooms)
        {
            return;
        }

        if (UnitManager.Instance.GetEnemyUnitList().Count == 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    private void Door_OnAnyDoorOpened(object sender, EventArgs e)
    {
        NextRoom();
    }

    public void NextTurn()
    {
        isPlayerTurn = !isPlayerTurn;

        if (isPlayerTurn)
        {
            turnNumber++;

            if (turnNumber > turnLimit)
            {
                GameManager.Instance.GameOver();
            }
        }

        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }

    public int GetTurnLimit()
    {
        return turnLimit;
    }

    private void NextRoom()
    {
        roomArray[currentRoom].SetActive(true);

        currentRoom++;

        turnLimit = turnLimitArray[currentRoom];
        turnNumber = 1;

        OnNewRoomEntered?.Invoke(this, EventArgs.Empty);
    }
}
