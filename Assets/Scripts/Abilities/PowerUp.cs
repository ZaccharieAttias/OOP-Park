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
            if (previousMethods[i].Name == "MoveSpeed") speedBuff.DeactivatePower(gameObject);
            if (previousMethods[i].Name == "GravityForce") gravityBuff.DeactivatePower(gameObject);
            if (previousMethods[i].Name == "DoubleJump") multipleJumpsBuff.DeactivatePower(gameObject);

            List<CharacterMethod> tempList = new List<CharacterMethod>(previousMethods);
            tempList.RemoveAt(i);
            previousMethods = tempList.ToArray();
        }

        foreach (CharacterMethod method in character.Methods)
        {
            if (method.Name == "MoveSpeed") speedBuff.ActivatePower(gameObject, method.Attribute.Value);
            if (method.Name == "GravityForce") gravityBuff.ActivatePower(gameObject,method.Attribute.Value);
            if (method.Name == "DoubleJump") multipleJumpsBuff.ActivatePower(gameObject,method.Attribute.Value);

            List<CharacterMethod> tempList = new List<CharacterMethod>(previousMethods);
            tempList.Add(method);
            previousMethods = tempList.ToArray();
        }
    }
}