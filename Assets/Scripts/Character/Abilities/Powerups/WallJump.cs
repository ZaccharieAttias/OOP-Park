using UnityEngine;


[CreateAssetMenu(menuName = "PowerUpEffects/WallJump")]
public class WallJump : PowerupEffect
{   
    public int NbrofJump = 0;

    public override void ActivatePower(GameObject player, float value)
    {
        NbrofJump = (int)value;
        player.GetComponent<PlayerMovement>().AllowToWallJump = true;
    }

    public override void DeactivatePower(GameObject player) { player.GetComponent<PlayerMovement>().AllowToWallJump = false;}
}
