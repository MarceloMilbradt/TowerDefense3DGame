using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShopSystem : Singleton<ShopSystem>
{
    [SerializeField] private List<Tower> towerList;
    [SerializeField] private Transform shopContainer;
    [SerializeField] private Transform towerButton;
    private void Awake()
    {
        CreateInstance(this);
    }
    private void Start()
    {
        foreach (var tower in towerList.OrderBy((t) => t.GetGoldCost()))
        {
            Transform actionButton = Instantiate(towerButton, shopContainer);
            Button actionButtonUI = actionButton.GetComponent<Button>();
            actionButtonUI.SetTower(tower);
        }
    }
}
