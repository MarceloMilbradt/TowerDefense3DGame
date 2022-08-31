using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitVfx;
    [SerializeField] private float moveSpeed;

    private IEntity target;
    private Vector3 targetPosition;
    private bool completed;
    [SerializeField] Enemy enemy;
    public void Setup(IEntity target)
    {
        this.target = target;
        if (target is Enemy enemy)
        {
            this.enemy = enemy;
        }
    }
    private void Update()
    {
        if (completed) return;

        if (enemy.isActiveAndEnabled)
        {
            SetTargetPosition(target.GetWorldPosition());
        }

        if (LevelGrid.Instance.GetGridPosition(targetPosition) == GridPosition.Zero) return;
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);
        transform.position += moveSpeed * Time.deltaTime * moveDir;
        transform.LookAt(targetPosition);
        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);
        if (distanceBeforeMoving < distanceAfterMoving)
        {
            Complete(true);
        }
    }

    internal void Destroy(Vector3 position)
    {
        SetTargetPosition(position);
    }
    private void SetTargetPosition(Vector3 pos)
    {
        if (pos != Vector3.zero && LevelGrid.Instance.GetGridPosition(pos) != GridPosition.Zero)
        {
            targetPosition = pos;
            targetPosition.y = 0.5f;
        }
    }
    private void Complete(bool hit)
    {
        if (completed) return;

        completed = true;

        Destroy(gameObject);
        if (hit)
            Instantiate(bulletHitVfx, targetPosition, Quaternion.identity);
    }
}
