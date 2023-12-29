using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.EventSystems;

public class LinesController : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    public List<Transform> _linePoints;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _linePoints = new List<Transform>();
    }

    public void Update()
    {
        for (int i = 0; i < _linePoints.Count; i++)
        {
            Debug.Log("iiiiiiiiiiiiiiiiiiii" + i);
            _lineRenderer.SetPosition(i, _linePoints[i].position);
        }
    }

    public void SetUpLine(List<Transform> linePoints)
    {
        _linePoints = linePoints;
        Debug.Log("SetUpLine" + _linePoints.Count);
        _lineRenderer.positionCount = _linePoints.Count;
    }

    public List<GameObject> GetLinePoints()
    {
        List<GameObject> transformCharacter = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "Character")
            {
                transformCharacter.Add(transform.GetChild(i).gameObject);
            }
        }
        return transformCharacter;
    }
}
