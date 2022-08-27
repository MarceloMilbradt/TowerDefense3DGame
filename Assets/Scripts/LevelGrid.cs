using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelGrid : Singleton<LevelGrid>
{
    private GridSystem<GridObject> gridSystem;
    [SerializeField] Transform debugTransform;
    public event EventHandler<IEntity> OnAnyEntityChangedGridPosition;
    [SerializeField] private bool debug;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;

    private void Awake()
    {
        CreateInstance(this);

        gridSystem = new GridSystem<GridObject>(width, height, cellSize, (grid, position) => new GridObject(grid, position));
        if (debug)
            gridSystem.CreateDebugObjects(debugTransform);
    }
    private void Start()
    {
        Pathfinding.Instance.Setup(width, height, cellSize);
    }

    public void AddAtGridPosition(GridPosition gridPosition, IEntity entity)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        entity.SetGridPosition(gridPosition);
        gridObject.Add(entity);
    }

    public List<IEntity> GetListAtGridPosition(GridPosition gridPosition)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetList();
    }
    public List<T> GetListOfAtGridPosition<T>(GridPosition gridPosition)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        var list = new List<T>();
        foreach (var entity in gridObject.GetList())
        {
            if (entity is T)
            {
                list.Add((T)entity);
            }
        }
        return list;
    }

    public void RemoveAtGridPosition(GridPosition gridPosition, IEntity entity)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.Remove(entity);
    }
    public void EntityMovedGridPosition(IEntity entity, GridPosition from, GridPosition to)
    {
        if (from != GridPosition.Empty)
        {
            RemoveAtGridPosition(from, entity);
        }
        if (to != GridPosition.Empty)
        {
            AddAtGridPosition(to, entity);
        }
        OnAnyEntityChangedGridPosition?.Invoke(this, entity);
    }
    public int GetWidth() => gridSystem.GetWidth();
    public int GetHeight() => gridSystem.GetHeight();
    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
    public Vector3 GetWorldPosition(GridPosition position) => gridSystem.GetWorldPosition(position);
    public List<GridPosition> GetGridPositions() => gridSystem.GetGridPositions();
    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);
    public bool HasAnyOnPosition(GridPosition gridPosition)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAny();
    }
    public IEntity GetAtPosition(GridPosition gridPosition)
    {
        var gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.Get();
    }
}
