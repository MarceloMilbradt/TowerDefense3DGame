using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform gridVisualTransform;
    private GridVisualSingle[,] gridVisuals;
    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType visualType;
        public Material material;
    }
    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        Green,
        Yellow
    }
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterials;
    void Start()
    {
        int width = LevelGrid.Instance.GetWidth();
        int hight = LevelGrid.Instance.GetHeight();
        gridVisuals = new GridVisualSingle[width, hight];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < hight; z++)
            {
                GridPosition pos = new GridPosition(x, z);
                var transform = Instantiate(gridVisualTransform, LevelGrid.Instance.GetWorldPosition(pos), Quaternion.identity);
                gridVisuals[x, z] = transform.GetComponent<GridVisualSingle>();
            }
        }
        LevelGrid.Instance.OnAnyEntityChangedGridPosition += LevelGrid_OnAnyEntityChangedGridPosition; ;
        TowerSystem.Instance.OnSelectedTowerChange += TowerSystem_OnSelectedTowerChange;
        UpdateGridVisual();
    }

    private void LevelGrid_OnAnyEntityChangedGridPosition(object sender, IEntity e)
    {
        if (e is not null && e is Tower)
        {
            UpdateGridVisual();
        }
    }

    private void TowerSystem_OnSelectedTowerChange(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }


    public void HideAllGridPositions()
    {
        int width = LevelGrid.Instance.GetWidth();
        int hight = LevelGrid.Instance.GetHeight();
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < hight; z++)
            {
                gridVisuals[x, z].Hide();
            }
        }
    }
    public void ShowGridPositions(List<GridPosition> positions, GridVisualType visualType)
    {
        foreach (GridPosition pos in positions)
        {

            gridVisuals[pos.x, pos.z].Show(GetGridVisualMaterial(visualType));
        }

    }

    public void UpdateGridVisual()
    {
        HideAllGridPositions();
        if (TowerSystem.Instance.HasSelectedTower())
        {
            var tower = TowerSystem.Instance.GetSelectedTower();
            if (!tower.TryGetComponent(out TowerAttack towerAttack))
            {
                return;
            }
            ShowGridPositions(towerAttack.GetPositionsInRange(), GridVisualType.White);

        }
        else
        {
            var validPositions = LevelGrid.Instance.GetGridPositions().Where(GridValidator.ValidateGridPosition).ToList();

            ShowGridPositions(validPositions, GridVisualType.White);
        }
    }
    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType visualType)
    {
        List<GridPosition> gridPositions = new List<GridPosition>();
        for (int x = -range; x < range; x++)
        {
            for (int z = -range; z < range; z++)
            {
                GridPosition position = gridPosition + new GridPosition(x, z);

                if (!GridValidator.ValidateGridPosition(position, GridValidation.None)) return;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);

                if (testDistance > range) continue;
                gridPositions.Add(position);
            }
        }

        ShowGridPositions(gridPositions, visualType);
    }
    private Material GetGridVisualMaterial(GridVisualType gridVisual)
    {
        foreach (GridVisualTypeMaterial visualTypeMaterial in gridVisualTypeMaterials)
        {
            if (visualTypeMaterial.visualType == gridVisual)
            {
                return visualTypeMaterial.material;
            }
        }
        throw new Exception("Material Not Found");
    }
}
