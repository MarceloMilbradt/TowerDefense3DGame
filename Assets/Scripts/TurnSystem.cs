using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum TurnState
{
    Waiting,
    Begining,
    Running,
    Ending,
}
public class TurnSystem : Singleton<TurnSystem>
{

    private int turnNumber;

    public event EventHandler<int> OnTurnChange;
    public event EventHandler<TurnState> OnTurnStateChange;

    public event EventHandler OnTurnRun;

    public event EventHandler OnTurnBegin;

    public event EventHandler OnTurnEnd;

    public event EventHandler OnTurnWaiting;


    [SerializeField] private float turnCooldown;
    [SerializeField] private float turnTime;

    private float stateTimer;
    private bool hasStarted;
    private float timeToNextState;
    private TurnState turnState;

    private void Awake()
    {
        CreateInstance(this);
    }
    public int GetTurn()
    {
        return turnNumber;
    }
    public float GetCountDown()
    {
        return timeToNextState - stateTimer;
    }

    private void LateUpdate()
    {
        if (!hasStarted) return;
        stateTimer += Time.deltaTime;
        Debug.Log(turnState);
        if (stateTimer >= timeToNextState && timeToNextState > 0)
        {
            NextState();
        }
    }
    private void NextState()
    {
        turnState = turnState switch
        {
            TurnState.Waiting => HandleTurnWaiting(),
            TurnState.Begining => HandleTurnBegining(),
            TurnState.Running => HandleTurnRunning(),
            TurnState.Ending => HandleTurnEnding(),
            _ => HandleTurnWaiting(),
        };
        stateTimer = 0;
        OnTurnStateChange?.Invoke(this, turnState);
    }

    private TurnState HandleTurnEnding()
    {
        timeToNextState = 0.1f;
        OnTurnEnd?.Invoke(this, EventArgs.Empty);

        return TurnState.Waiting;
    }

    private TurnState HandleTurnRunning()
    {
        timeToNextState = turnTime;
        OnTurnRun?.Invoke(this, EventArgs.Empty);

        return TurnState.Ending;
    }

    private TurnState HandleTurnBegining()
    {
        timeToNextState = 2;
        turnNumber++;

        OnTurnChange?.Invoke(this, turnNumber);
        OnTurnBegin?.Invoke(this, EventArgs.Empty);
        return TurnState.Running;
    }

    private TurnState HandleTurnWaiting()
    {
        timeToNextState = turnCooldown;

        OnTurnWaiting?.Invoke(this, EventArgs.Empty);
        return TurnState.Begining;
    }

    public bool TryEndTurn()
    {
        if (!hasStarted)
        {
            return false;
        }
        if (turnState != TurnState.Ending)
        {
            return false;
        }
        NextState();
        return true;
    }
    public void StartGame()
    {
        hasStarted = true;
        turnState = TurnState.Begining;
        NextState();
    }
    public void StopGame()
    {
        hasStarted = false;
        turnState = TurnState.Waiting;
    }
}
