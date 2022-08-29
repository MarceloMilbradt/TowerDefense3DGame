using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSystem : Singleton<TowerSystem>
{
    public event EventHandler OnSelectedTowerChange;

    [SerializeField] private Tower selectedTower;
    [SerializeField] private LayerMask towerLayer;
    [SerializeField] private LayerMask gridLayer;

    private void Awake()
    {
        CreateInstance(this);
    }
    private void Update()
    {
        //if (EventSystem.current.IsPointerOverGameObject()) return;
        if (TryHandleTowerSelection()) return;
        if (TryHandleCreateTower()) return;

    }
    private void SetSelectedTower(Tower tower)
    {
        selectedTower = tower;
        OnSelectedTowerChange?.Invoke(this, EventArgs.Empty);
    }

    public Tower GetSelectedTower() { return selectedTower; }
    public bool HasSelectedTower() { return selectedTower != null; }
    private bool TryHandleTowerSelection()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetSelectedTower(null);
            return false;
        }
        if (!Input.GetMouseButtonDown(0))
        {
            return false;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, towerLayer);
        if (!hit)
        {
            return false;
        }
        Debug.DrawRay(ray.origin, ray.direction, Color.magenta, float.MaxValue);
        var position = raycastHit.transform.position;
        var gridPosition = LevelGrid.Instance.GetGridPosition(position);
        if (!LevelGrid.Instance.HasAnyOnPosition(gridPosition))
        {
            return false;
        }
        var tower = (Tower)LevelGrid.Instance.GetAtPosition(gridPosition);
        if (tower == selectedTower)
        {
            return false;
        }

        SetSelectedTower(tower);
        return true;
    }
    private bool TryHandleCreateTower()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return false;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, gridLayer);

        if (!hit)
        {
            return false;
        }

        var gridSystem = LevelGrid.Instance;
        var pathSystem = PathSystem.Instance;

        var gridPosition = gridSystem.GetGridPosition(MouseWorld.GetPosition());

        if (!GridValidator.ValidateGridPosition(gridPosition)) return false;

        Tower tower = ShopSystem.Instance.BuyTower();
        if (tower is null) return false;

        var towerTransform = Instantiate(tower.transform, gridSystem.GetWorldPosition(gridPosition), Quaternion.identity);
        var towerComponent = towerTransform.GetComponent<Tower>();
        gridSystem.EntityMovedGridPosition(towerComponent, GridPosition.Empty, gridPosition);
        SetSelectedTower(towerComponent);
        return true;
    }
}
