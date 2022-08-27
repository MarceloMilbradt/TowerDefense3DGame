using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTowerSystem : Singleton<GridTowerSystem>
{
    [SerializeField] private Transform towerPrefab;
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

            var towerTransform = Instantiate(towerPrefab, gridSystem.GetWorldPosition(gridPosition), Quaternion.identity);
            var tower = towerTransform.GetComponent<Tower>();
            gridSystem.AddAtGridPosition(gridPosition, tower);

        }
    }
}
