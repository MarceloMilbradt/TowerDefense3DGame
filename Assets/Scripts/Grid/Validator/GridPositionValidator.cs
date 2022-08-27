public class GridPositionValidator : IGridValidator
{
    public bool Validate(GridPosition position)
    {
        return LevelGrid.Instance.IsValidGridPosition(position);
    }
}
