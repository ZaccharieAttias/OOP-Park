using UnityEngine;


[CreateAssetMenu(menuName = "PowerUpEffects/WallJump")]
public class WallJump : PowerupEffect
{
    public int NbrofJump = 0;

    public override void ActivatePower(GameObject player, float value)
    {
        NbrofJump = (int)value;
        player.GetComponent<Movement>().AllowToWallJump = true;
    }
    public override void plusActivatePower(GameObject player) {  }

    public override void plusDeactivatePower(GameObject player) {  }

    public override void DeactivatePower(GameObject player) { player.GetComponent<Movement>().AllowToWallJump = false; }
}
