using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUpEffects/GravityBuff")]

public class GravityBuff : PowerUpEffect
{   
    public float jumpforceeffect = 20f;

    public override void ActivatePower(GameObject player)
    {
        player.GetComponent<PlayerMovement>().jumpForce += jumpforceeffect;
    }

    public override void DeactivatePower(GameObject player)
    {
        player.GetComponent<PlayerMovement>().jumpForce -= jumpforceeffect;
    }
}
