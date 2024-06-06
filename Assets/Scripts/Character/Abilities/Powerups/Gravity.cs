using UnityEngine;


[CreateAssetMenu(menuName = "PowerUpEffects/Gravity")]
public class Gravity : PowerupEffect
{   
    public float JumpForceIncrease = 20f;


    public override void ActivatePower(GameObject player, float value)
    {
        JumpForceIncrease = value;
        player.GetComponent<Movement>().JumpForce += JumpForceIncrease;
    }

    public override void DeactivatePower(GameObject player) { player.GetComponent<Movement>().JumpForce -= JumpForceIncrease; }
}
