using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform pathTransform;
    private bool pathDrawn;
    private void Update()
    {
        if (pathDrawn) return;

        var path = PathSystem.Instance.GetUniquePath();
        DrawPath(path);
    }

    public void DrawPath(List<GridPosition> path)
    {
        foreach (var item in path)
        {
            var transform = Instantiate(pathTransform, LevelGrid.Instance.GetWorldPosition(item), Quaternion.identity);
        }
        pathDrawn = true;
    }
}
