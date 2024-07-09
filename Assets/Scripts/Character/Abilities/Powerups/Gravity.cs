using UnityEngine;


[CreateAssetMenu(menuName = "PowerUpEffects/Gravity")]
public class Gravity : PowerupEffect
{
    public float JumpForceIncrease = 0f;


    public override void ActivatePower(GameObject player, float value)
    {
        JumpForceIncrease = value;
        player.GetComponent<Movement>().JumpForce += JumpForceIncrease;
    }
    public override void plusActivatePower(GameObject player) { player.GetComponent<Movement>().JumpForce += 3f; }

    public override void plusDeactivatePower(GameObject player) { player.GetComponent<Movement>().JumpForce -= 3f; }
    public override void DeactivatePower(GameObject player) { player.GetComponent<Movement>().JumpForce -= JumpForceIncrease; }
}
