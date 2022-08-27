using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathSystem : Singleton<PathSystem>
{
    public event EventHandler<List<GridPosition>> OnPathCreated;

    [SerializeField] private List<Vector2> targetPaths;
    private List<GridPosition> gridPositionPaths;

    private void Awake()
    {
        CreateInstance(this);
    }

    private void Start()
    {
        gridPositionPaths = new List<GridPosition>();
        Pathfinding pathfinding = Pathfinding.Instance;
        List<GridPosition> gridPositions = new List<GridPosition>();
        foreach (var target in targetPaths)
        {
            gridPositions.Add(new GridPosition(target.x, target.y));

        }
        for (int i = 0; i < gridPositions.Count - 1; i++)
        {
            var nextPosition = gridPositions[i + 1];
            var currentPosition = gridPositions[i];

            var pathToPosition = pathfinding.FindPath(currentPosition, nextPosition, out _);
            gridPositionPaths.AddRange(pathToPosition);
        }
        OnPathCreated?.Invoke(this, gridPositionPaths);
    }

    public List<GridPosition> GetPath()
    {
        return gridPositionPaths;
    }
    public List<GridPosition> GetUniquePath()
    {
        return gridPositionPaths.Distinct().ToList();
    }
    public bool IsPath(GridPosition position)
    {
        return gridPositionPaths.Contains(position);
    }
}
