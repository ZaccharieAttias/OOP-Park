using UnityEngine;


[CreateAssetMenu(menuName = "PowerUpEffects/GravityBuff")]
public class GravityBuff : PowerUpEffect
{   
    public float JumpForceEffect = 20f;


    public override void ActivatePower(GameObject player, float value)
    {
        JumpForceEffect = value;
        player.GetComponent<PlayerMovement>().jumpForce += JumpForceEffect;
    }

    public override void DeactivatePower(GameObject player) { player.GetComponent<PlayerMovement>().jumpForce -= JumpForceEffect; }
}
