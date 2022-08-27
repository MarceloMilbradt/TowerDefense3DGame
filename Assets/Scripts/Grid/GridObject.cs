using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridSystem<GridObject> gridSystem;
    private GridPosition gridPosition;
    private List<IEntity> entities;
    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        entities = new List<IEntity>();
    }
    public void Add(IEntity IEntity)
    {
        entities.Add(IEntity);
    }
    public void Remove(IEntity IEntity)
    {
        entities.Remove(IEntity);
    }
    public List<IEntity> GetList()
    {
        return entities;
    }
    public bool HasAny()
    {
        return entities.Count > 0;
    }
    public IEntity Get()
    {
        if (HasAny())
        {
            return entities[0];
        }
        return null;
    }
    public GridPosition GetPosition() => gridPosition;
    public override string ToString()
    {
        var position = gridPosition.ToString();
        return $"{position}\n{entities.Count}";
    }
}
