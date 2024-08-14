using UnityEngine;


[CreateAssetMenu(menuName = "PowerUpEffects/Speed")]
public class Speed : PowerupEffect
{
    public float SpeedIncrease = 0f;


    public override void ActivatePower(GameObject player, float value)
    {
        SpeedIncrease = value;
        player.GetComponent<Movement>().MoveSpeed += SpeedIncrease;
    }

    public override void plusActivatePower(GameObject player) { player.GetComponent<Movement>().MoveSpeed += 15f; }

    public override void plusDeactivatePower(GameObject player) { player.GetComponent<Movement>().MoveSpeed -= 15; }
    public override void DeactivatePower(GameObject player) { player.GetComponent<Movement>().MoveSpeed -= SpeedIncrease; }
}
