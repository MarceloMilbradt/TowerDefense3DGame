using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    public class OnShootEventAgrs : EventArgs
    {
        public IEntity target;
        public IEntity origin;
    }
    public static event EventHandler<OnShootEventAgrs> OnAnyShoot;
    public event EventHandler<OnShootEventAgrs> OnShoot;
    private List<GridPosition> range;
    private TowerStats stats;
    private Tower tower;
    private List<Enemy> targets;
    private enum State
    {
        FindingTargets,
        Shooting,
        Cooldown
    }
    private State state;
    private float stateTimer;
    private bool canShoot;

    private void Awake()
    {
        targets = new List<Enemy>();
        stats = GetComponent<TowerStats>();
        tower = GetComponent<Tower>();
    }
    public void AddTarget(Enemy enemy) { targets.Add(enemy); }
    public void RemoveTarget(Enemy enemy) { targets.Remove(enemy); }

    private void Update()
    {
        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.FindingTargets:
                ValidateTargets();
                break;
            case State.Shooting:
                foreach (var target in targets.OrderByDescending( t => t.GetDistance()))
                {
                    Shoot(target);
                }

                break;
            case State.Cooldown:
                break;
        }

        if (targets.Count == 0 && state == State.FindingTargets) return;

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }
    private void Shoot(Enemy target)
    {

        OnShoot?.Invoke(this, new OnShootEventAgrs
        {
            target = target,
            origin = tower
        });
        OnAnyShoot?.Invoke(this, new OnShootEventAgrs
        {
            target = target,
            origin = tower
        });

        target.Damage(stats.GetDamage());
    }

    private void NextState()
    {
        switch (state)
        {
            case State.FindingTargets:
                state = State.Shooting;
                stateTimer = 0.0f;

                break;
            case State.Shooting:
                state = State.Cooldown;
                stateTimer = stats.GetAttackSpeed();

                break;
            case State.Cooldown:
                state = State.FindingTargets;
                stateTimer = 0.0f;
                break;
        }
    }


    public List<GridPosition> GetPositionsInRange()
    {
        if (range != null)
            return range;
        List<GridPosition> validPositions = new List<GridPosition>();
        GridPosition unitGridPostion = tower.GetGridPosition();
        var maxDistance = stats.GetRange();
        for (int x = -maxDistance; x <= maxDistance; x++)
        {
            for (int z = -maxDistance; z <= maxDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPostion + offsetGridPosition;
                if (!GridValidator.ValidateGridPosition(testGridPosition, GridValidation.Position)) continue;

                //var pathLength = Pathfinding.Instance.GetPathLength(unitGridPostion, testGridPosition);

                //if (pathLength > maxDistance * 15) continue; //Muito Longe

                validPositions.Add(testGridPosition);
            }
        }


        range = validPositions;
        return validPositions;
    }
    public bool TryFindTarget()
    {
        var positionsInRange = GetPositionsInRange();
        foreach (var positions in positionsInRange)
        {
            if (targets.Count >= stats.GetProjectileNumber()) break;
            if (!LevelGrid.Instance.HasAnyOnPosition(positions)) continue;
            var enemies = LevelGrid.Instance.GetListOfAtGridPosition<Enemy>(positions);
            if (enemies.Count == 0) continue;
            var enemyFound = enemies[0];
            if (targets.Contains(enemyFound)) continue;
            AddTarget(enemyFound);
        }
        return targets.Count > 0;
    }

    private void ValidateTargets()
    {
        var positionsInRange = GetPositionsInRange();
        if (targets.Count == 0)
        {
            TryFindTarget();
            return;
        }
        targets = targets.Distinct().ToList();

        foreach (var target in new List<Enemy>(targets))
        {
            if (target.IsAlive() && positionsInRange.Contains(target.GetGridPosition())) continue;
            targets.Remove(target);
        }

        if (targets.Count < stats.GetProjectileNumber())
        {
            TryFindTarget();
        }

        return;
    }
}
