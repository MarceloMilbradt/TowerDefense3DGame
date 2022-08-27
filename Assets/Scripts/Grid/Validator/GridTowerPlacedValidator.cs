public class GridTowerPlacedValidator : IGridValidator
{
    private IGridValidator _validator;
    public GridTowerPlacedValidator(IGridValidator validator)
    {
        _validator = validator;
    }

    public bool Validate(GridPosition position)
    {
        return _validator.Validate(position) && !LevelGrid.Instance.HasAnyOnPosition(position);
    }
}
