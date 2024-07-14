using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    public GameObject Player;
    public Vector3 PositionOffset;
    public Vector3 Velocity;
    public float TimeOffset;

    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        if (!Player) Player = GameObject.Find("Player");
        PositionOffset = new Vector3(0, 0, -10);
        Velocity = Vector3.zero;
        TimeOffset = 0.2f;
    }
    
    public void Update() 
    { 
        if (Player) 
            {
                //Restrict the camera in the y-axis to -2 and 2 and SmoothDamp the camera to the player's position
                //transform.position = new Vector3(Mathf.Clamp(transform.position.x, -53, 53), Mathf.Clamp(transform.position.y, -2, 2), transform.position.z);
                transform.position = Vector3.SmoothDamp(transform.position, Player.transform.position + PositionOffset, ref Velocity, TimeOffset);
                
            } 

    }
    public void ResetPosition()
    {
        transform.position = new Vector3(0, 0, -0.3f);
    }
}
