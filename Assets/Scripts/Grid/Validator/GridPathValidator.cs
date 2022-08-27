public class GridPathValidator : IGridValidator
{
    private IGridValidator _validator;
    public GridPathValidator(IGridValidator validator)
    {
        _validator = validator;
    }

    public bool Validate(GridPosition position)
    {
        return _validator.Validate(position) && !PathSystem.Instance.IsPath(position);
    }
}
