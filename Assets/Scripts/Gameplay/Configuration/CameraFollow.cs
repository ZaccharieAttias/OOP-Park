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
        Player = GameObject.Find("New character");
        PositionOffset = new Vector3(0, 0, -10);
        Velocity = Vector3.zero;

        TimeOffset = 0.2f;
    }
    
    public void Update() { transform.position = Vector3.SmoothDamp(transform.position, Player.transform.position + PositionOffset, ref Velocity, TimeOffset); }
}
