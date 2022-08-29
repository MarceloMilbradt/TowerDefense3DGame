using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private Button button;
    private Tower tower;
    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            Debug.Log(tower);
            ShopSystem.Instance.TrySetSelectedTower(tower);
        });

    }
    public void SetTower(Tower tower)
    {
        this.tower = tower;
        cost.text = tower.GetGoldCost().ToString();
        icon.sprite = tower.GetSprite();
    }
    public bool CanBuy(int gold)
    {
        return tower.GetGoldCost() <= gold;
    }

    internal void SetEnabled(bool enabled)
    {
        button.interactable = enabled;
    }
}
