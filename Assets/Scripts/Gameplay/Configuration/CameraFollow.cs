using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    public GameObject Player;
    public Vector3 PositionOffset;
    public Vector3 Velocity;
    public float TimeOffset;
    public Vector3 StartPosition;
    public Vector3 MinValues, MaxValues;

    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        if (!Player) Player = GameObject.Find("Player");
        PositionOffset = new Vector3(0, 0, -10);
        Velocity = Vector3.zero;
        TimeOffset = 0.215f;
        StartPosition = new Vector3(0, 0, -10);
    }
    
    public void Update() 
    { 
        if (Player) 
        {
            Vector3 targetPosition = Player.transform.position + PositionOffset;
            Vector3 boundPosition = new Vector3(
                Mathf.Clamp(targetPosition.x, MinValues.x, MaxValues.x), 
                Mathf.Clamp(targetPosition.y, MinValues.y, MaxValues.y), 
                Mathf.Clamp(targetPosition.z, MinValues.z, MaxValues.z));
            
            transform.position = Vector3.SmoothDamp(transform.position, boundPosition, ref Velocity, TimeOffset);
            
        }

    }
    public void ResetPosition()
    {
        transform.position = StartPosition;
    }
}
