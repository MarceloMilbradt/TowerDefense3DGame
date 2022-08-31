using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public event EventHandler<float> OnChange;
    public event EventHandler OnDeath;
    [SerializeField] private int damagePerUnit = 2;
    private HealthSystem healthSystem;
    void Awake()
    {
        CreateInstance(this);
        healthSystem = GetComponent<HealthSystem>();
    }
    private void Start()
    {
        EndNode.OnAnyUnitPass += EndNode_OnAnyUnitPass;
        healthSystem.OnDie += (object sender, EventArgs e) =>
        {
            OnDeath?.Invoke(this, EventArgs.Empty);
        };
    }

    private void EndNode_OnAnyUnitPass(object sender, System.EventArgs e)
    {
        healthSystem.Damage(damagePerUnit);
        OnChange?.Invoke(this, healthSystem.GetHealthNormalized());
    }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }
    public int GetHealth()
    {
        return healthSystem.GetHealth();
    }

}
