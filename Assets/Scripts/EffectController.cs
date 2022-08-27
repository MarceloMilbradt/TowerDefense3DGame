using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EffectController : MonoBehaviour
{
    [SerializeField] private VisualEffect effect;
    public void Fire()
    {
        effect.SendEvent("Activate");
    }
    public void Stop()
    {
        effect.SendEvent("Stop");
    }
}
