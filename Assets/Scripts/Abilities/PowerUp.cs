using System.Collections.Generic;
using UnityEngine;


public class PowerUp : MonoBehaviour
{
    public SpeedBuff SpeedBuff;
    public GravityBuff GravityBuff;
    public MultipleJumpsBuff MultipleJumpsBuff;

    public List<CharacterMethod> PreviousMethods;


    public void Start() { InitializeProperties(); }
    public void InitializeProperties()
    {
        SpeedBuff = Resources.Load<SpeedBuff>("Prefabs/PowerUps/SpeedBuff");
        GravityBuff = Resources.Load<GravityBuff>("Prefabs/PowerUps/GravityBuff");
        MultipleJumpsBuff = Resources.Load<MultipleJumpsBuff>("Prefabs/PowerUps/MultipleJumpsBuff");

        PreviousMethods = new List<CharacterMethod>();
    }

    public void ApplyPowerup(Character character)
    {
        foreach (CharacterMethod method in PreviousMethods)
        {
            if (method.Name == "MoveSpeed") SpeedBuff.DeactivatePower(gameObject);
            if (method.Name == "GravityForce") GravityBuff.DeactivatePower(gameObject);
            if (method.Name == "DoubleJump") MultipleJumpsBuff.DeactivatePower(gameObject);

            PreviousMethods.Remove(method);
        }

        foreach (CharacterMethod method in character.Methods)
        {
            if (method.Name == "MoveSpeed") SpeedBuff.ActivatePower(gameObject, method.Attribute.Value);
            if (method.Name == "GravityForce") GravityBuff.ActivatePower(gameObject, method.Attribute.Value);
            if (method.Name == "DoubleJump") MultipleJumpsBuff.ActivatePower(gameObject, method.Attribute.Value);

            PreviousMethods.Add(method);
        }
    }
}
