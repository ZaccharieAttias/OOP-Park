using UnityEngine;


public abstract class PowerupEffect : ScriptableObject
{
    public abstract void ActivatePower(GameObject player, float value);
    public abstract void DeactivatePower(GameObject player);
    public abstract void plusActivatePower(GameObject player);
    public abstract void plusDeactivatePower(GameObject player);
}
