using System.Collections.Generic;
using UnityEngine;


public class Powerup : MonoBehaviour
{
    public List<PowerupEffect> PowerUpEffects;
    public List<CharacterMethod> PreviousMethods;
    public List<CharacterUpcastMethod> PreviousUpcastMethods;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void OnGameStart()
    {
        Powerup powerUp = GameObject.Find("Player").GetComponent<Powerup>();
        powerUp.InitializeProperties();
    }
    private void InitializeProperties()
    {
        PowerUpEffects = new List<PowerupEffect>();
        PowerUpEffects.AddRange(Resources.LoadAll<PowerupEffect>("PowerUps"));

        PreviousMethods = new List<CharacterMethod>();
        PreviousUpcastMethods = new List<CharacterUpcastMethod>();
    }
    
    public void ApplyPowerup(Character character)
    {
        if (PreviousUpcastMethods?.Count > 0)
        {
            for (int i = 0; i < PreviousUpcastMethods.Count; i++)
            {
                PowerUpEffects.Find(powerup => powerup.GetType().Name.Contains(PreviousUpcastMethods[i].CharacterMethod.Name)).DeactivatePower(gameObject);
                PreviousUpcastMethods[i].UpdateUpcast();
                PreviousUpcastMethods.Remove(PreviousUpcastMethods[i]);
            }
        }

        if (PreviousMethods?.Count > 0)
        {
            for (int i = 0; i < PreviousMethods.Count; i++)
            {
                PowerUpEffects.Find(powerup => powerup.GetType().Name.Contains(PreviousMethods[i].Name)).DeactivatePower(gameObject);
                PreviousMethods.Remove(PreviousMethods[i]);
            }
        }

        if (character.UpcastMethods?.Count > 0)
        {
            for (int i = 0; i < character.UpcastMethods.Count; i++)
            {
                PowerUpEffects.Find(powerup => powerup.GetType().Name.Contains(character.UpcastMethods[i].CharacterMethod.Name)).ActivatePower(gameObject, character.UpcastMethods[i].CharacterMethod.Attribute.Value);
                PreviousUpcastMethods.Add(character.UpcastMethods[i]);
            }
        }

        if (character.Methods?.Count > 0)
        {
            for (int i = 0; i < character.Methods.Count; i++)
            {
                if (PreviousUpcastMethods.Find(upcast => upcast.CharacterMethod.Name == character.Methods[i].Name) != null) continue;
                PowerUpEffects.Find(powerup => powerup.GetType().Name.Contains(character.Methods[i].Name)).ActivatePower(gameObject, character.Methods[i].Attribute.Value);
                PreviousMethods.Add(character.Methods[i]);
            }
        }
    }
}