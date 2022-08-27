using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour, IEntity
{
    private TowerAttack attack;
    private void Awake()
    {
        attack = GetComponent<TowerAttack>();
    }
    private GridPosition gridPosition;
    public void SetGridPosition(GridPosition gridPosition) { this.gridPosition = gridPosition; }
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public bool IsEnemy()
    {
        return false;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
}
