using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAim : MonoBehaviour
{
    [SerializeField] private Transform aim;
    private Enemy mainTarget;
    private bool hasTarget;

    public void SetTarget(Enemy enemy)
    {
        mainTarget = enemy;
        hasTarget = true;
        enemy.OnDeath += Enemy_OnDeath;
    }

    private void Enemy_OnDeath(object sender, System.EventArgs e)
    {
        var enemy = sender as Enemy;
        if (enemy == mainTarget)
        {
            hasTarget = false;
        }
        enemy.OnDeath -= Enemy_OnDeath;
    }

    private void Update()
    {
        if (hasTarget)
        {
            Vector3 moveDirection = (mainTarget.GetWorldPosition() - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * 100f);
        }
    }
}
