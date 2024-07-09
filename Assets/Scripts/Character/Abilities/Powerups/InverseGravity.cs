using UnityEngine;


[CreateAssetMenu(menuName = "PowerUpEffects/InverseGravity")]
public class InverseGravity : PowerupEffect
{
    public float JumpForceIncrease = 0f;


    public override void ActivatePower(GameObject player, float value)
    {
        JumpForceIncrease = value;
        player.GetComponent<Movement>().JumpForce += JumpForceIncrease;
        player.GetComponent<Rigidbody2D>().gravityScale = -player.GetComponent<Rigidbody2D>().gravityScale;
        player.transform.localScale = new Vector3(player.transform.localScale.x, -player.transform.localScale.y, player.transform.localScale.z);
    }

    public override void plusActivatePower(GameObject player) { player.GetComponent<Movement>().JumpForce -= 5f; }

    public override void plusDeactivatePower(GameObject player) { player.GetComponent<Movement>().JumpForce += 5; }
    public override void DeactivatePower(GameObject player)
    {
        player.GetComponent<Movement>().JumpForce -= JumpForceIncrease;
        player.GetComponent<Rigidbody2D>().gravityScale = -player.GetComponent<Rigidbody2D>().gravityScale;
        player.transform.localScale = new Vector3(player.transform.localScale.x, -player.transform.localScale.y, player.transform.localScale.z);
    }
}
