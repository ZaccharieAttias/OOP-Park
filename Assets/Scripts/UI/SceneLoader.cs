using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void Presentation()
    {
        SceneManager.LoadScene("Presentation");
    }

    public void Playground()
    {
        SceneManager.LoadScene("Playground");
    }

    public void LocalPark()
    {
        SceneManager.LoadScene("LocalPark");
    }

    public void OnlinePark()
    {
        SceneManager.LoadScene("OnlinePark");
    }

    public void Progression()
    {
        SceneManager.LoadScene("Progression");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void Hierarchy()
    {
        SceneManager.LoadScene("Hierarchy");
    }

    public void Polymorphism()
    {
        SceneManager.LoadScene("Polymorphism");
    }
}
