using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUpEffects/SpeedBuff")]

public class SpeedBuff : PowerUpEffect
{   
    public float speedIncrease = 5f;

    public override void ActivatePower(GameObject player)
    {
        player.GetComponent<PlayerMovement>().moveSpeed += speedIncrease;
    }
}
