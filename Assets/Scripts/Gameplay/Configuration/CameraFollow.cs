using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    public GameObject Player;
    public Vector3 PositionOffset = new Vector3(0, 0, -10);
    public Vector3 Velocity = Vector3.zero;
    public float TimeOffset = 0.215f;
    public Vector3 StartPosition;
    public Vector3 MinValues, MaxValues;

    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        if (!Player) Player = GameObject.Find("Player");
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
        else
        {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, MinValues.x, MaxValues.x),
                Mathf.Clamp(transform.position.y, MinValues.y, MaxValues.y),
                Mathf.Clamp(transform.position.z, MinValues.z, MaxValues.z));
        }

    }
    public void ResetPosition()
    {
        transform.position = StartPosition;
    }
}
