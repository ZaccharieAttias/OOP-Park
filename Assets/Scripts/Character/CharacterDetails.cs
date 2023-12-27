using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterDetails : MonoBehaviour
{
    public Character character;

    public void InitializeCharacter(Character character)
    {
        this.character = character;
    }
}
