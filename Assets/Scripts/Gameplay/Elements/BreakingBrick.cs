using UnityEngine;


public class BreakingBrick : MonoBehaviour
{
    [Header("Animator")]
    public Animator Animator;


    public void Start()
    {
        InitializeComponents();
    }
    public void InitializeComponents()
    {
        Animator = GetComponent<Animator>();
    }

    public void Break()
    {
        Animator.SetTrigger("Break");
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    public void Deactivate()
    {
        Animator.SetTrigger("Deactivate");
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Activate()
    {
        gameObject.SetActive(true);
        Animator.SetTrigger("Activate");
    }
}