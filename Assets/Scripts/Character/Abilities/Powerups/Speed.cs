using UnityEngine;


[CreateAssetMenu(menuName = "PowerUpEffects/Speed")]
public class Speed : PowerupEffect
{
    public float SpeedIncrease = 5f;


    public override void ActivatePower(GameObject player, float value)
    {
        SpeedIncrease = value;
        player.GetComponent<PlayerMovement>().MoveSpeed += SpeedIncrease;
    }

    public override void DeactivatePower(GameObject player) { player.GetComponent<PlayerMovement>().MoveSpeed -= SpeedIncrease; }
}
