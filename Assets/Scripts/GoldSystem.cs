using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldSystem : Singleton<GoldSystem>
{
    [SerializeField] private int startingGold;
    public event EventHandler<int> OnChange;

    private int goldCount;
    public int GetGold()
    {
        return goldCount;
    }
    public void AddGold(int amount)
    {
        SetGold(goldCount + amount);
    }
    public void SpendGold(int amount)
    {
        SetGold(goldCount - amount);
    }
    private void SetGold(int gold)
    {
        goldCount = gold;
        OnChange?.Invoke(this, GetGold());
    }
    public bool CanSpendGold(int amount)
    {
        return amount <= goldCount;
    }
    private void Awake()
    {
        goldCount = startingGold;
        CreateInstance(this);
    }
    private void Start()
    {
        Enemy.OnAnyDeath += Enemy_OnAnyDeath;
    }

    private void Enemy_OnAnyDeath(object sender, Enemy e)
    {
        AddGold(e.GetReward());
    }
}
