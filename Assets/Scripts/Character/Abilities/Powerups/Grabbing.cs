using UnityEngine;


[CreateAssetMenu(menuName = "PowerUpEffects/Grabbing")]
public class Grabbing : PowerupEffect
{
    public float MassIncrease = 0f;


    public override void ActivatePower(GameObject player, float value)
    {
        MassIncrease = value;
        player.GetComponent<GrabObject>().CanGrabMass += MassIncrease;
    }
    public override void plusActivatePower(GameObject player) { player.GetComponent<GrabObject>().CanGrabMass += 1000f; }
    public override void DeactivatePower(GameObject player) { player.GetComponent<GrabObject>().CanGrabMass -= MassIncrease; }
    public override void plusDeactivatePower(GameObject player) { player.GetComponent<GrabObject>().CanGrabMass -= 1000f; }
}
