using UnityEngine;
public interface IEntity
{
    public bool IsEnemy();
    public GridPosition GetGridPosition();
    public void SetGridPosition(GridPosition gridPosition);
    Vector3 GetWorldPosition();
}
