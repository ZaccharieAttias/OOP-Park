using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingBrick : MonoBehaviour
{    //break the brick
    public void Break()
    {
        GetComponent<Animator>().SetTrigger("Break");
    }
    //destroy the brick
    public void Destroy()
    {
        Destroy(gameObject);
    }
    public void Deactivate()
    {
        GetComponent<Animator>().SetTrigger("Deactivate");
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Activate()
    {
        gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger("Activate");

    }
}
