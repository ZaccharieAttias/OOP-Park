using UnityEngine;


[CreateAssetMenu(menuName = "PowerUpEffects/FireballShoot")]
public class FireballShoot : PowerupEffect
{
    public float attackCooldown = 0.25f;


    public override void ActivatePower(GameObject player, float value)
    {
        player.GetComponent<Movement>().attackCooldown = attackCooldown;
    }

    public override void DeactivatePower(GameObject player) { player.GetComponent<Movement>().attackCooldown = Mathf.Infinity; player.GetComponent<Movement>().cooldownTimer = Mathf.Infinity;}
}
