using UnityEngine;


public class MovingPlatform : MonoBehaviour
{
    public Transform PointA;
    public Transform PointB;
    public Vector2 Position;

    public float Speed;

    public void Start() { InitializeProperties(); }
    private void InitializeProperties()
    {
        PointA = GameObject.Find("Grid/MovingPlatforms/PointA").GetComponent<Transform>();
        PointB = GameObject.Find("Grid/MovingPlatforms/PointB").GetComponent<Transform>();
        Position = PointA.position;

        Speed = 2f;
    }

    public void Update()
    {
        if (Vector2.Distance(transform.position, PointA.position) < 0.5f) Position = PointB.position;
        if (Vector2.Distance(transform.position, PointB.position) < 0.5f) Position = PointA.position;
        
        transform.position = Vector3.MoveTowards(transform.position, Position, Speed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D collision) { collision.transform.parent = collision.CompareTag("Player") ? transform : null; }
    public void OnTriggerExit2D(Collider2D collision) { collision.transform.parent = collision.CompareTag("Player") ? null : transform; }
}
