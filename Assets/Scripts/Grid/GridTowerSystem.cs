using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Obsolete("Use TowerSystem")]
public class GridTowerSystem : Singleton<GridTowerSystem>
{
    private void Awake()
    {
        CreateInstance(this);
    }

    private void Update()
    {
        var gridSystem = LevelGrid.Instance;
        var pathSystem = PathSystem.Instance;
        if (Input.GetMouseButtonDown(0))
        {
            var gridPosition = gridSystem.GetGridPosition(MouseWorld.GetPosition());

            if (!GridValidator.ValidateGridPosition(gridPosition)) return;
            Tower tower = ShopSystem.Instance.BuyTower();

            if (tower is null) return;

            var towerTransform = Instantiate(tower.transform, gridSystem.GetWorldPosition(gridPosition), Quaternion.identity);
            gridSystem.AddAtGridPosition(gridPosition, tower);

        }
    }
}
