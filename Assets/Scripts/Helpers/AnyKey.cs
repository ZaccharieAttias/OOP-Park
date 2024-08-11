using UnityEngine;


public class AnyKey : MonoBehaviour
{
    [SerializeField] private TransitionManager transitionManager;
    void Update()
    {
        if (Input.anyKey)
        {
            transitionManager.EnableEndingSceneTransition("Playground");
        }
    }
}
