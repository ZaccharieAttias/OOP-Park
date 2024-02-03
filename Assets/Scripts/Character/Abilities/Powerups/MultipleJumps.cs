using UnityEngine;


[CreateAssetMenu(menuName = "PowerUpEffects/MultipleJumps")]
public class MultipleJumps : PowerUpEffect
{   
    public int JumpIncrease = 1;


    public override void ActivatePower(GameObject player, float value)
    {
        JumpIncrease = (int)value;
        player.GetComponent<PlayerMovement>().MaxJumps += JumpIncrease;
    }

    public override void DeactivatePower(GameObject player) { player.GetComponent<PlayerMovement>().MaxJumps -= JumpIncrease; }
}
