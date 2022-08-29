using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Button : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI cost;
    private Tower tower;
    public void SetTower(Tower tower)
    {
        this.tower = tower;
        cost.text = tower.GetGoldCost().ToString();
        icon.sprite = tower.GetSprite();
    }

}
