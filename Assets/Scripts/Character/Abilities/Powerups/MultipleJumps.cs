using UnityEngine;


[CreateAssetMenu(menuName = "PowerUpEffects/MultipleJumps")]
public class MultipleJumps : PowerupEffect
{   
    public int JumpIncrease = 1;


    public override void ActivatePower(GameObject player, float value)
    {
        JumpIncrease = (int)value;
        player.GetComponent<Movement>().MaxJumps += JumpIncrease;
    }

    public override void DeactivatePower(GameObject player) { player.GetComponent<Movement>().MaxJumps -= JumpIncrease; }
}
