using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    public GameObject Player;
    public Vector3 PositionOffset;
    public Vector3 Velocity;

    public float TimeOffset;
    

    void Update() { transform.position = Vector3.SmoothDamp(transform.position, Player.transform.position + PositionOffset, ref Velocity, TimeOffset); }
}
