using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected void CreateInstance(T instance)
    {
        if (Instance != null)
        {
            Debug.LogError($"There is more than one {nameof(T)} {transform} - {Instance}");
            Destroy(gameObject);
            return;
        }
        Instance = instance;
    }
}
