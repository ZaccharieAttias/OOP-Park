using UnityEngine;


[CreateAssetMenu(menuName = "PowerUpEffects/MutipleJumpsBuff")]
public class DoubleJumpsBuff : PowerUpEffect
{   
    public int NewJumpsAmount = 1;


    public override void ActivatePower(GameObject player, float value)
    {
        NewJumpsAmount = (int)value;
        player.GetComponent<PlayerMovement>().amountOfJumps += NewJumpsAmount;
    }

    public override void DeactivatePower(GameObject player) { player.GetComponent<PlayerMovement>().amountOfJumps -= NewJumpsAmount; }
}
