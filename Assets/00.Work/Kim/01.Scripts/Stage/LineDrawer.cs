using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer;

    private Vector3 _startPos, _endPos;

    public void DrawLine(Vector3 start, Vector3 end)
    {
        _startPos = start;
        _endPos = end;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
