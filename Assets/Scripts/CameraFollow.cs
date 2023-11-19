using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float timeOffset;
    public Vector3 posOffset;

    private Vector3 velocity;
    public CameraFollow instance;

    void Start()
    {
        if(instance != null)
        {
            Debug.Log("camerafollow instance already exists");
            Destroy(this.gameObject);
            return;
        }
        Debug.Log("camerafollow instance created");
        instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
    
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + posOffset, ref velocity, timeOffset);
    }
}