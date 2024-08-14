using UnityEngine;
using UnityEngine.SceneManagement;


public class GrabObject : MonoBehaviour
{
    [Header("Objects")]
    public Transform GrabPoint;
    public Transform RaycastPoint;
    public GameObject ObjectInHand;
    public LayerMask GrabMask;
    public Powerup Powerup;

    [Header("Settings")]
    public float RaycastDistance;
    public float CanGrabMass;
    public float PowerupTimer;

    [Header("State")]
    public bool IsHolding;
    public bool CanHold;


    public void Start()
    {
        InitializeObjects();
        InitializeSettings();
        InitializeState();
    }
    public void Update()
    {
        HandleRaycast();
        UpdatePowerupTimer();
    }
    public void InitializeObjects()
    {
        GrabPoint = transform.Find("GrabPosition").transform;
        RaycastPoint = transform.Find("RayGrabPosition").transform;
        ObjectInHand = null;
        GrabMask = LayerMask.GetMask("Grabbable");
        Powerup = GameObject.Find("Scripts/PowerUp").GetComponent<Powerup>();
    }
    public void InitializeSettings()
    {
        RaycastDistance = 1f;
        CanGrabMass = 0f;
        PowerupTimer = 0f;
    }
    public void InitializeState()
    {
        IsHolding = false;
        CanHold = true;
    }

    public void HandleRaycast()
    {
        RaycastDistance = transform.localScale.x > 0 ? 0.5f : -0.5f;
        RaycastHit2D hit = Physics2D.Raycast(RaycastPoint.position, transform.right, RaycastDistance);
        Debug.DrawRay(RaycastPoint.position, transform.right * RaycastDistance, Color.red);

        RaycastHit2D grabpointHitLeft = Physics2D.Raycast(GrabPoint.position, -transform.right, 0.6f);
        RaycastHit2D grabpointHitRight = Physics2D.Raycast(GrabPoint.position, transform.right, 0.6f);
        Debug.DrawRay(GrabPoint.position, -transform.right * RaycastDistance, Color.green);
        Debug.DrawRay(GrabPoint.position, transform.right * RaycastDistance, Color.green);

        if (hit.collider != null && IsValidGrab(hit))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Grabbing(hit);
            }
        }
        else if (IsHolding && grabpointHitLeft.collider == null && grabpointHitRight.collider == null && Input.GetKeyDown(KeyCode.E))
        {
            Drop();
        }
    }
    public bool IsValidGrab(RaycastHit2D hit)
    {
        return GrabMask == (GrabMask | (1 << hit.collider.gameObject.layer)) &&
               CanHold &&
               !IsHolding &&
               hit.collider.gameObject.GetComponent<Rigidbody2D>().mass < CanGrabMass;
    }

    public void UpdatePowerupTimer()
    {
        PowerupTimer += Time.deltaTime;

        if (Powerup.PreviousUpcastMethod?.CharacterMethod.Name == "Grabbing")
        {
            Powerup.PreviousUpcastMethod.UpcastingTrackerManager.UpdateUpcastingMethod(PowerupTimer);
        }
    }

    public void Grabbing(RaycastHit2D hit)
    {
        ObjectInHand = hit.collider.gameObject;
        ObjectInHand.GetComponent<Rigidbody2D>().isKinematic = true;
        ObjectInHand.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ObjectInHand.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        ObjectInHand.transform.rotation = Quaternion.Euler(0, 0, 0);
        ObjectInHand.transform.position = GrabPoint.position;
        ObjectInHand.transform.parent = GameObject.Find("Player").transform;
        ObjectInHand.GetComponent<BoxMovement>().ResetTimers();
        ObjectInHand.GetComponent<BoxCollider2D>().isTrigger = true;
        IsHolding = true;
        CanHold = false;
    }

    public void Drop()
    {
        ObjectInHand.GetComponent<Rigidbody2D>().isKinematic = false;
        ObjectInHand.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x*1.5f, 3f);
        if (SceneManager.GetActiveScene().name.Contains("Online")) ObjectInHand.transform.parent = GameObject.Find("Grid/LevelBuilder/Gameplay").transform;
        else ObjectInHand.transform.parent = GameObject.Find("Grid/Gameplay").transform;
        ObjectInHand.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        ObjectInHand.GetComponent<BoxCollider2D>().isTrigger = false;

        ObjectInHand = null;
        IsHolding = false;
        CanHold = true;
    }

    public void ResetGrab()
    {
        PowerupTimer = 0f;
        CanGrabMass = 0f;
        if (ObjectInHand != null)
            Drop();
        IsHolding = false;
        CanHold = true;
    }
}
