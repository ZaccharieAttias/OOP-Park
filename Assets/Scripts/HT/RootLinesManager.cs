using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.EventSystems;

public class RootLinesManager : MonoBehaviour
{
    [SerializeField] private List<Transform> _linePoints;
    [SerializeField] private LinesController _linesController;
    [SerializeField] private List<GameObject> _transformCharacter;

    public void Start()
    {
        _linePoints = new List<Transform>();
        _linesController = GameObject.Find("Canvas/HTMenu/Menu/Characters/Tree/Buttons").GetComponent<LinesController>();
        _transformCharacter = _linesController.GetLinePoints();
    }

    public void SetUp(GameObject point)
    {
        Debug.Log("SetUp");
        _linePoints.Add(point.transform);
        Character character = point.GetComponent<CharacterDetails>().GetCurrentCharacter();     
        if (character.parents.Count == 0) 
        {
            Debug.Log("iciciciici");
            if (character.childrens.Count == 0)
            {
                Debug.Log("child 0");
                return;
            }
            SetUp(_transformCharacter.Find(item => item.GetComponent<CharacterDetails>().GetCurrentCharacter().name == character.name).gameObject);
            return;
        }
        if (character.childrens.Count == 0)
        {  
            _linePoints.Add(point.transform);
            Debug.Log("fffff" + _linePoints.Count);
            _linesController.SetUpLine(_linePoints);
            _linePoints.Clear();
            return;
        }
        
        for (int i = 0; i < character.childrens.Count; i++)
        {
            _linePoints.Add(_transformCharacter.Find(item => item.GetComponent<CharacterDetails>().GetCurrentCharacter().name == character.childrens[i].name).transform);
            SetUp(_transformCharacter.Find(item => item.GetComponent<CharacterDetails>().GetCurrentCharacter().name == character.childrens[i].name));
        }
    }
}
