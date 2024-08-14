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
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2.34f, player.transform.position.z);
    }

    public override void plusActivatePower(GameObject player) { player.GetComponent<Movement>().JumpForce -= 60f; }

    public override void plusDeactivatePower(GameObject player) { player.GetComponent<Movement>().JumpForce += 60; }
    public override void DeactivatePower(GameObject player)
    {
        player.GetComponent<Movement>().JumpForce -= JumpForceIncrease;
        player.GetComponent<Rigidbody2D>().gravityScale = -player.GetComponent<Rigidbody2D>().gravityScale;
        player.transform.localScale = new Vector3(player.transform.localScale.x, -player.transform.localScale.y, player.transform.localScale.z);
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 2.34f, player.transform.position.z);
    }
}
