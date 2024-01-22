using UnityEngine;


[CreateAssetMenu(menuName = "PowerUpEffects/SpeedBuff")]
public class SpeedBuff : PowerUpEffect
{
    public float SpeedIncrease = 5f;


    public override void ActivatePower(GameObject player, float value)
    {
        SpeedIncrease = value;
        player.GetComponent<PlayerMovement>().moveSpeed += SpeedIncrease;
    }

    public override void DeactivatePower(GameObject player) { player.GetComponent<PlayerMovement>().moveSpeed -= SpeedIncrease; }
}
