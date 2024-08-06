using UnityEngine;


[CreateAssetMenu(menuName = "PowerUpEffects/WallJump")]
public class WallJump : PowerupEffect
{
    public int NbrofJump = 0;

    public override void ActivatePower(GameObject player, float value)
    {
        NbrofJump = (int)value;
        player.GetComponent<Movement>().AllowToWallJump = true;
        player.GetComponent<Movement>().MaxWallJumps = NbrofJump;
    }
    public override void plusActivatePower(GameObject player) { player.GetComponent<Movement>().MaxWallJumps += 1; }

    public override void plusDeactivatePower(GameObject player) { player.GetComponent<Movement>().MaxWallJumps -= 1; }

    public override void DeactivatePower(GameObject player) { player.GetComponent<Movement>().AllowToWallJump = false; player.GetComponent<Movement>().MaxWallJumps = 7; }
}