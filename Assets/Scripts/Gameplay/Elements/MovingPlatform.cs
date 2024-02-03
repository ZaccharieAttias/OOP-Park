using UnityEngine;


public class MovingPlatform : MonoBehaviour
{
    public Transform PointA;
    public Transform PointB;
    public Vector2 PlatformPosition;

    public float Speed;


    public void Start() { InitializeProperties(); }
    private void InitializeProperties() { PlatformPosition = PointA.position; }

    public void Update()
    {
        if (Vector2.Distance(transform.position, PointA.position) < 0.5f) PlatformPosition = PointB.position;
        if (Vector2.Distance(transform.position, PointB.position) < 0.5f) PlatformPosition = PointA.position;
        
        transform.position = Vector3.MoveTowards(transform.position, PlatformPosition, Speed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D collision) { collision.transform.parent = collision.CompareTag("Player") ? transform : null; }
    public void OnTriggerExit2D(Collider2D collision) { collision.transform.parent = collision.CompareTag("Player") ? null : transform; }
}
