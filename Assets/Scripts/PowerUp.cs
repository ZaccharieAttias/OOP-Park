using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public SpeedBuff speedBuff;
    public CharacterMethod[] previousMethods;

    public void ApplyPowerup(Character character)
    {
        for (int i = previousMethods.Length - 1; i >= 0; i--)
        {
            if (previousMethods[i].name == "MoveSpeed")
            {
                speedBuff.DeactivatePower(gameObject);
            }

            List<CharacterMethod> tempList = new List<CharacterMethod>(previousMethods);
            tempList.RemoveAt(i);
            previousMethods = tempList.ToArray();
        }

        foreach (CharacterMethod method in character.methods)
        {
            if (method.name == "MoveSpeed")
            {
                speedBuff.ActivatePower(gameObject);
            }

            List<CharacterMethod> tempList = new List<CharacterMethod>(previousMethods);
            tempList.Add(method);
            previousMethods = tempList.ToArray();
        }
    }
}
