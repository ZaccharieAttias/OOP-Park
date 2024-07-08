using System.Collections.Generic;
using UnityEngine;


public class Powerup : MonoBehaviour
{
    public List<PowerupEffect> PowerUpEffects;
    public List<Method> PreviousMethods;
    public UpcastMethod PreviousUpcastMethod;
    public GameObject Player;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        PowerUpEffects = new List<PowerupEffect>(Resources.LoadAll<PowerupEffect>("Powerups"));
        PowerUpEffects.AddRange(Resources.LoadAll<PowerupEffect>("Powerups"));

        PreviousMethods = new List<Method>();
        PreviousUpcastMethod = null;
        if (GameObject.Find("Player")) Player = GameObject.Find("Player");
    }

    public void ApplyPowerup(CharacterB character)
    {
        if (Player == null) Player = GameObject.Find("Player");
        if (PreviousUpcastMethod != null)
        {
            PowerUpEffects.Find(powerup => powerup.GetType().Name.Contains(PreviousUpcastMethod.CharacterMethod.Name)).DeactivatePower(Player);
            PreviousUpcastMethod = null;
        }

        if (PreviousMethods?.Count > 0)
        {
            for (int i = PreviousMethods.Count - 1; i >= 0; i--)
            {
                PowerUpEffects.Find(powerup => powerup.GetType().Name.Contains(PreviousMethods[i].Name)).DeactivatePower(Player);
                PreviousMethods.RemoveAt(i);
            }
        }

        if (character.UpcastMethod != null)
        {
            PowerUpEffects.Find(powerup => powerup.GetType().Name.Contains(character.UpcastMethod.CharacterMethod.Name)).ActivatePower(Player, character.UpcastMethod.CharacterMethod.Attribute.Value);
            PreviousUpcastMethod = character.UpcastMethod;
        }

        if (character.Methods?.Count > 0)
        {
            for (int i = 0; i < character.Methods.Count; i++)
            {
                if (PreviousUpcastMethod?.CharacterMethod?.Name == character.Methods[i].Name) continue;
                PowerUpEffects.Find(powerup => powerup.GetType().Name.Contains(character.Methods[i].Name)).ActivatePower(Player, character.Methods[i].Attribute.Value);
                PreviousMethods.Add(character.Methods[i]);
            }
        }
    }
}
