using UnityEngine;


[CreateAssetMenu(menuName = "PowerUpEffects/Gravity")]
public class Gravity : PowerupEffect
{   
    public float JumpForceIncrease = 20f;


    public override void ActivatePower(GameObject player, float value)
    {
        JumpForceIncrease = value;
        player.GetComponent<PlayerMovement>().JumpForce += JumpForceIncrease;
    }

    public override void DeactivatePower(GameObject player) { player.GetComponent<PlayerMovement>().JumpForce -= JumpForceIncrease; }
}
