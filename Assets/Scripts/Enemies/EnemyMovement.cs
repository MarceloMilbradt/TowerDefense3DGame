using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField][Range(0f, 5f)] float speed = 1f;
    private List<Vector3> positionList;
    private bool active;
    private int currentPositionIndex;
    private float maxDistance;
    private float currentDistance;
    public void Stop()
    {
        active = false;
    }
    public void Setup(List<GridPosition> positions)
    {
        currentPositionIndex = 0;
        positionList = new List<Vector3>();
        foreach (var position in positions)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(position));
        }
        GetMaxDistance();
        active = true;
    }
    public float GetDistanceNormalized()
    {
        return currentPositionIndex / positionList.Count;
    }
    private void Update()
    {
        if (!active) return;
        float stoppingDistance = .1f;
        Vector3 targetPosition = positionList[currentPositionIndex];

        var distance = Vector3.Distance(transform.position, targetPosition);
        currentDistance = distance;
        bool isMoving = distance >= stoppingDistance;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        if (isMoving)
        {
            transform.position += speed * Time.deltaTime * moveDirection;
        }
        if (!isMoving)
        {
            currentPositionIndex++;
            GetMaxDistance();
        }
    }
    private void GetMaxDistance()
    {
        Vector3 targetPosition = positionList[currentPositionIndex];
        var distance = Vector3.Distance(transform.position, targetPosition);
        maxDistance = distance;
    }
}
