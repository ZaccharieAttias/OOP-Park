using UnityEngine;


[CreateAssetMenu(menuName = "PowerUpEffects/GrapplingGun")]
public class GrapplingGun :  PowerupEffect
{
    public float GrabblingNumber = 10f;


    public override void ActivatePower(GameObject player, float value)
    {
        GrabblingNumber = value;
        player.transform.Find("GunPivot").gameObject.SetActive(true);
        player.GetComponent<SpringJoint2D>().enabled = true;
    }

    public override void DeactivatePower(GameObject player) 
    {
        player.transform.Find("GunPivot").gameObject.SetActive(false); 
        player.GetComponent<SpringJoint2D>().enabled = false;
    }
}
