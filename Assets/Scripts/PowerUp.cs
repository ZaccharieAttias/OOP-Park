using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public SpeedBuff speedBuff;
    public Character previousCharacter=null;

    public void ApplyPowerup(Character character)
    {
        // if (!(previousCharacter?.name == null || previousCharacter?.name == "" || previousCharacter?.name == " "))
        if(previousCharacter is not null)
        {
            Debug.Log("Previous character is not null");
            foreach (CharacterMethod method in previousCharacter.methods)
            {
                if (method.name == "MoveSpeed")
                {
                    Debug.Log("Previous character has MoveSpeed");
                    speedBuff.DeactivatePower(gameObject);
                }
            }
        }


        foreach (CharacterMethod method in character.methods)
        {
            if (method.name == "MoveSpeed")
            {
                Debug.Log("Current character has MoveSpeed");
                speedBuff.ActivatePower(gameObject);
            }
        }

        previousCharacter = character;
    }
}
