using UnityEngine;


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
        RaycastDistance = 0.6f;
        CanGrabMass = 10f;
        PowerupTimer = 0f;
    }
    public void InitializeState()
    {
        IsHolding = false;
        CanHold = true;
    }

    public void HandleRaycast()
    {
        RaycastDistance = transform.localScale.x > 0 ? 0.6f : -0.6f;
        RaycastHit2D hit = Physics2D.Raycast(RaycastPoint.position, transform.right, RaycastDistance);
        Debug.DrawRay(RaycastPoint.position, transform.right * RaycastDistance, Color.red);

        if (hit.collider != null && IsValidGrab(hit))
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Grabbing(hit);
            }
        }
        else if (IsHolding && Input.GetKeyDown(KeyCode.Q))
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
        ObjectInHand.transform.position = GrabPoint.position;
        ObjectInHand.transform.parent = GameObject.Find("Player").transform;
        IsHolding = true;
        CanHold = false;
    }

    public void Drop()
    {
        ObjectInHand.GetComponent<Rigidbody2D>().isKinematic = false;
        ObjectInHand.transform.parent = GameObject.Find("Grid/Obstacles/Grabbables").transform;
        ObjectInHand = null;
        IsHolding = false;
        CanHold = true;
    }

    public void ResetGrab()
    {
        PowerupTimer = 0f;
        CanGrabMass = 10f;
        IsHolding = false;
        CanHold = true;
    }
}
