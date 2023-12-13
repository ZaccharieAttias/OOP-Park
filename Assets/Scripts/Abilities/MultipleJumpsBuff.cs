using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUpEffects/MutipleJumpsBuff")]

public class MultipleJumpsBuff : PowerUpEffect
{   
    public int amountOfNewJumps = 1;

    public override void ActivatePower(GameObject player)
    {
        player.GetComponent<PlayerMovement>().amountOfJumps += amountOfNewJumps;
    }

    public override void DeactivatePower(GameObject player)
    {
        player.GetComponent<PlayerMovement>().amountOfJumps -= amountOfNewJumps;
    }
}
