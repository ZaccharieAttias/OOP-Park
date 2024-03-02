using UnityEngine;


[CreateAssetMenu(menuName = "PowerUpEffects/Grabbing")]
public class Grabbing : PowerupEffect
{   
    public float MassIncrease = 50f;


    public override void ActivatePower(GameObject player, float value)
    {
        MassIncrease = value;
        player.GetComponent<GrabObject>().CanGrabMass += MassIncrease;
    }

    public override void DeactivatePower(GameObject player) { player.GetComponent<GrabObject>().CanGrabMass -= MassIncrease; }
}
