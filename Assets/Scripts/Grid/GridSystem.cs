using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem<TGridObject>
{
    private int height;
    private int width;
    private float cellSize;
    private TGridObject[,] gridObjects;
    public GridSystem(int width, int height, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.gridObjects = new TGridObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var gridPosition = new GridPosition(x, z);
                gridObjects[x, z] = createGridObject(this, gridPosition);
            }
        }
    }
    public List<GridPosition> GetGridPositions()
    {
        List<GridPosition> gridPositions = new List<GridPosition>();
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                gridPositions.Add(new GridPosition(x, z));
            }
        }
        return gridPositions;
    }
    public Vector3 GetWorldPosition(GridPosition position)
    {
        return new Vector3(position.x, 0, position.z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPos)
    {
        return new GridPosition(worldPos.x / cellSize, worldPos.z / cellSize);
    }
    public TGridObject GetGridObject(GridPosition position)
    {
        return gridObjects[position.x, position.z];
    }
    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var position = new GridPosition(x, z);
                Transform debugTransform = UnityEngine.Object.Instantiate(debugPrefab, GetWorldPosition(position), Quaternion.identity);
                GridDebugObject gridDebug = debugTransform.GetComponent<GridDebugObject>();
                gridDebug.SetGridObject(GetGridObject(position));
            }
        }
    }
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 &&
               gridPosition.z >= 0 &&
               gridPosition.x < width &&
               gridPosition.z < height;
    }
    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
}
