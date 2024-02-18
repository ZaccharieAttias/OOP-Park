using System.Collections.Generic;
using UnityEngine;


public class Powerup : MonoBehaviour
{
    public List<PowerupEffect> PowerUpEffects;
    public List<Method> PreviousMethods;
    public CharacterUpcastMethod PreviousUpcastMethod;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        PowerUpEffects = new List<PowerupEffect>(Resources.LoadAll<PowerupEffect>("Powerups"));
        PowerUpEffects.AddRange(Resources.LoadAll<PowerupEffect>("Powerups"));

        PreviousMethods = new List<Method>();
        PreviousUpcastMethod = null;
    }
    
    public void ApplyPowerup(Character character)
    {
        if (PreviousUpcastMethod != null)
        {
            PowerUpEffects.Find(powerup => powerup.GetType().Name.Contains(PreviousUpcastMethod.CharacterMethod.Name)).DeactivatePower(gameObject);
            PreviousUpcastMethod = null;
        }

        if (PreviousMethods?.Count > 0)
        {
            for (int i = 0; i < PreviousMethods.Count; i++)
            {
                PowerUpEffects.Find(powerup => powerup.GetType().Name.Contains(PreviousMethods[i].Name)).DeactivatePower(gameObject);
                PreviousMethods.Remove(PreviousMethods[i]);
            }
        }

        if (character.UpcastMethod != null)
        {
            PowerUpEffects.Find(powerup => powerup.GetType().Name.Contains(character.UpcastMethod.CharacterMethod.Name)).ActivatePower(gameObject, character.UpcastMethod.CharacterMethod.Attribute.Value);
            PreviousUpcastMethod = character.UpcastMethod;
        }

        if (character.Methods?.Count > 0)
        {
            for (int i = 0; i < character.Methods.Count; i++)
            {
                if (PreviousUpcastMethod?.CharacterMethod?.Name == character.Methods[i].Name) continue;
                PowerUpEffects.Find(powerup => powerup.GetType().Name.Contains(character.Methods[i].Name)).ActivatePower(gameObject, character.Methods[i].Attribute.Value);
                PreviousMethods.Add(character.Methods[i]);
            }
        }
    }
}
