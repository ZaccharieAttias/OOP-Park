using UnityEngine;


public abstract class PowerUpEffect : ScriptableObject
{
    public abstract void ActivatePower(GameObject player, float value);
    public abstract void DeactivatePower(GameObject player);
}
