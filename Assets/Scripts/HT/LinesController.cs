using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesController : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private List<Transform> _linePoints;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _linePoints = new List<Transform>();
    }

    public void SetUpLine(List<Transform> linePoints)
    {
        _linePoints = linePoints;
        _lineRenderer.positionCount = _linePoints.Count;
        DrawLines();
    }

    private void DrawLines()
    {
        for (int i = 0; i < _linePoints.Count; i++)
        {
            _lineRenderer.SetPosition(i, _linePoints[i].position);
        }
    }
}
