using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ShopSystem : Singleton<ShopSystem>
{
    [SerializeField] private List<Tower> towerList;
    [SerializeField] private Transform shopContainer;
    [SerializeField] private Transform towerButton;
    private Tower selectedTower;
    private List<TowerButton> buttons;
    private void Awake()
    {
        CreateInstance(this);
        buttons = new List<TowerButton>();
    }
    private void Start()
    {
        GoldSystem.Instance.OnChange += GoldSystem_OnChange;
        int gold = GoldSystem.Instance.GetGold();
        foreach (var tower in towerList.OrderBy((t) => t.GetGoldCost()))
        {
            Transform actionButton = Instantiate(towerButton, shopContainer);
            TowerButton actionButtonUI = actionButton.GetComponent<TowerButton>();
            actionButtonUI.SetTower(tower);
            actionButtonUI.SetEnabled(actionButtonUI.CanBuy(gold));
            buttons.Add(actionButtonUI);
        }
    }

    private void GoldSystem_OnChange(object sender, int e)
    {
        foreach (var btn in buttons)
        {
            var affordable = btn.CanBuy(e);
            btn.SetEnabled(affordable);
        }
    }
    public Tower BuyTower()
    {
        if (selectedTower is null) return null;
        GoldSystem.Instance.SpendGold(selectedTower.GetGoldCost());
        var tower = selectedTower;
        selectedTower = null;
        return tower;
    }
    internal bool TrySetSelectedTower(Tower tower)
    {
        selectedTower = tower;
        return true;
    }
}
