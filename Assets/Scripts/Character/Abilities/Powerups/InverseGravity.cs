using UnityEngine;


[CreateAssetMenu(menuName = "PowerUpEffects/InverseGravity")]
public class InverseGravity : PowerupEffect
{   
    public float JumpForceIncrease = -40f;


    public override void ActivatePower(GameObject player, float value)
    {
        JumpForceIncrease = value;
        player.GetComponent<Movement>().JumpForce += JumpForceIncrease;
        player.GetComponent<Rigidbody2D>().gravityScale = -player.GetComponent<Rigidbody2D>().gravityScale;
        player.transform.localScale = new Vector3(player.transform.localScale.x, -player.transform.localScale.y, player.transform.localScale.z);
    }

    public override void DeactivatePower(GameObject player) 
    { 
        player.GetComponent<Movement>().JumpForce -= JumpForceIncrease;
        player.GetComponent<Rigidbody2D>().gravityScale = -player.GetComponent<Rigidbody2D>().gravityScale; 
        player.transform.localScale = new Vector3(player.transform.localScale.x, -player.transform.localScale.y, player.transform.localScale.z);
    }
}
