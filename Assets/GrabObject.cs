using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    public Transform GrabPoint;
    public Transform RaycastPoint;
    public float RaycastDistance;
    public GameObject ObjectInHand;
    public bool IsHolding;
    public bool CanHold;
    public LayerMask GrabMask;
    public float PowerupTimer = 0f;
    public float CanGrabMass = 10f;
    public Powerup Powerup;


    public void Start()
    {
        InitializeProperties();
    }

    public void InitializeProperties()
    {
        GrabPoint = GameObject.Find("Player/GrabPosition").transform;
        RaycastPoint = GameObject.Find("Player/RayGrabPosition").transform;
        RaycastDistance = 0.6f;
        IsHolding = false;
        CanHold = true;
        GrabMask = LayerMask.GetMask("Grabbable");
        Powerup = GameObject.Find("Player").GetComponent<Powerup>();
    }

    public void Update()
    {
        PowerupTimer = 0f;
        if ( transform.localScale.x > 0)
            RaycastDistance = 0.6f; 
        else RaycastDistance = -0.6f;

        RaycastHit2D hit = Physics2D.Raycast(RaycastPoint.position, transform.right, RaycastDistance);
        PowerupTimer += Time.deltaTime;
        if (Powerup.PreviousUpcastMethod?.CharacterMethod.Name == "Grabbing") Powerup.PreviousUpcastMethod.UpcastingTrackerManager.UpdateUpcastingMethod(PowerupTimer);
        if (hit.collider != null && GrabMask == (GrabMask | (1 << hit.collider.gameObject.layer)) && CanHold && !IsHolding && hit.collider.gameObject.GetComponent<Rigidbody2D>().mass < CanGrabMass && Input.GetKeyDown(KeyCode.Q))
        {
            Grabbing(hit);
        }
        else if (IsHolding && Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }

        Debug.DrawRay(RaycastPoint.position, transform.right * RaycastDistance, Color.red);
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
}
