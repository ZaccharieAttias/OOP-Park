using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public SpeedBuff speedBuff;

    public void hhhhhhhhh(Character character)
    {
        foreach (CharacterMethod method in character.methods)
        {
            if (method.name == "MoveSpeed")
            {
                speedBuff.ActivatePower(gameObject);
            }
        }
    }
}
