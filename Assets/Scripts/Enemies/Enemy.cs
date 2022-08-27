using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEntity
{
    public event EventHandler OnDeath;
    public static event EventHandler<Enemy> OnAnyDeath;
    HealthSystem healthSystem;
    GridPosition gridPosition;
    [SerializeField] private int goldReward = 2;
    private List<Projectile> projectilesFollowing;
    public void AddProjectile(Projectile projectile) { projectilesFollowing.Add(projectile); }
    private void Awake()
    {
        projectilesFollowing = new List<Projectile>();
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDie += HealthSystem_OnDie;
        gridPosition = GridPosition.Empty;
    }

    private void HealthSystem_OnDie(object sender, EventArgs e)
    {

        ClearProjectiles(GetWorldPosition());
        LevelGrid.Instance.EntityMovedGridPosition(this, gridPosition, GridPosition.Empty);
        OnAnyDeath?.Invoke(this, this);
        OnDeath?.Invoke(this, EventArgs.Empty);
        gameObject.SetActive(false);
    }

    public void ClearProjectiles(Vector3 vector)
    {
        foreach (var projectile in projectilesFollowing)
        {
            projectile.Destroy(vector);
        }
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public bool IsEnemy()
    {
        return true;
    }

    public void SetGridPosition(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }
    public int GetReward()
    {
        return goldReward;
    }

    private void Update()
    {
        var newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (!LevelGrid.Instance.IsValidGridPosition(newGridPosition)) return;
        if (gridPosition != newGridPosition)
        {
            var oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.EntityMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    internal void Damage(int value)
    {
        healthSystem.Damage(value);
    }

    internal bool IsAlive()
    {
        return healthSystem.IsAlive();
    }
}
