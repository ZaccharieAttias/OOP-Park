using System;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public List<PowerupEffect> PowerUpEffects;
    public List<Method> PreviousMethods;
    public UpcastMethod PreviousUpcastMethod;
    public GameObject Player;

    public SpecialAbility PreviousSpecialAbility;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        PowerUpEffects = new List<PowerupEffect>(Resources.LoadAll<PowerupEffect>("Powerups"));
        PowerUpEffects.AddRange(Resources.LoadAll<PowerupEffect>("Powerups"));

        PreviousMethods = new List<Method>();
        PreviousUpcastMethod = null;
        if (GameObject.Find("Player")) Player = GameObject.Find("Player");

        PreviousSpecialAbility = null;
    }

    public void ApplyPowerup(CharacterB character)
    {
        if (Player == null) Player = GameObject.Find("Player");

        if (RestrictionManager.Instance.AllowSpecialAbility && PreviousSpecialAbility != null)
        {
            var specialAbilityName = PreviousSpecialAbility.Name.Replace(" ", "");
            PowerUpEffects.Find(powerup => powerup.GetType().Name.Equals(specialAbilityName, StringComparison.OrdinalIgnoreCase)).plusDeactivatePower(Player);
            if (PowerUpEffects.Find(powerup => powerup.GetType().Name.Equals(specialAbilityName, StringComparison.OrdinalIgnoreCase)))
            PreviousSpecialAbility = null;
        }

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
                if (character.Methods[i].Name.Equals("Appearance", StringComparison.OrdinalIgnoreCase)) continue;
                PowerUpEffects.Find(powerup => powerup.GetType().Name.Contains(character.Methods[i].Name)).ActivatePower(Player, character.Methods[i].Attribute.Value);
                PreviousMethods.Add(character.Methods[i]);
            }
        }

        if (RestrictionManager.Instance.AllowSpecialAbility && character.SpecialAbility != null)
        {
            // if a special ability name contain spaces then remove the spaces
            var specialAbilityName = character.SpecialAbility.Name.Replace(" ", "");
            if (PreviousMethods.Exists(method => method.Name.Equals(specialAbilityName, StringComparison.OrdinalIgnoreCase)))
            {
                PowerUpEffects.Find(powerup => powerup.GetType().Name.Contains(specialAbilityName)).plusActivatePower(Player);
                PreviousSpecialAbility = character.SpecialAbility;
            }
        }
    }
}
