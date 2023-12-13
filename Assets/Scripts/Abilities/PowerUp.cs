using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public SpeedBuff speedBuff;
    public GravityBuff gravityBuff;
    public MultipleJumpsBuff multipleJumpsBuff;

    public CharacterMethod[] previousMethods;

    public void ApplyPowerup(Character character)
    {
        for (int i = previousMethods.Length - 1; i >= 0; i--)
        {
            if (previousMethods[i].name == "MoveSpeed")
            {
                speedBuff.DeactivatePower(gameObject);
            }
            if (previousMethods[i].name == "GravityForce")
            {
                gravityBuff.DeactivatePower(gameObject);
            }
            if (previousMethods[i].name == "DoubleJump")
            {
                multipleJumpsBuff.DeactivatePower(gameObject);
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
            if (method.name == "GravityForce")
            {
                gravityBuff.ActivatePower(gameObject);
            }
            if (method.name == "DoubleJump")
            {
                multipleJumpsBuff.ActivatePower(gameObject);
            }

            List<CharacterMethod> tempList = new List<CharacterMethod>(previousMethods);
            tempList.Add(method);
            previousMethods = tempList.ToArray();
        }
    }
}
