using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndNode : Singleton<EndNode>
{
    EffectController controller;
    private void Awake()
    {
        CreateInstance(this);
    }
    private void Start()
    {
        controller = GetComponent<EffectController>();
    }
    public static event EventHandler OnAnyUnitPass;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy _))
        {
            other.gameObject.SetActive(false);
            OnAnyUnitPass?.Invoke(this, EventArgs.Empty);
        }
    }
    public void StartEffect()
    {
        controller.Fire();
    }
    public void StopEffect()
    {
        controller.Stop();
    }
}
