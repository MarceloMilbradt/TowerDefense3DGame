using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum GridValidation
{
    None = 0,
    Position = 1,
    Path = 2,
    Tower = 4,
}
public static class GridValidator
{

    public static bool ValidateGridPosition(GridPosition position)
    {
        return ValidateGridPosition(position, GridValidation.Path | GridValidation.Tower);
    }

    public static bool ValidateGridPosition(GridPosition position, GridValidation validation)
    {

        if (validation == GridValidation.None) return true;
        IGridValidator validator;
        validator = new GridPositionValidator();

        if (validation.HasFlag(GridValidation.Path))
        {
            var pathValidator = new GridPathValidator(validator);
            validator = pathValidator;
        }

        if (validation.HasFlag(GridValidation.Tower))
        {
            var towerValidator = new GridTowerPlacedValidator(validator);
            validator = towerValidator;
        }

        return validator.Validate(position);
    }
}