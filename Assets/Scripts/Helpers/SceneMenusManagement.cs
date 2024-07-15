using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMenusManagement : MonoBehaviour
{  
    public void Presentation() { SceneManager.LoadScene("Presentation"); }
    public void Playground() { SceneManager.LoadScene("Playground"); }
    public void LocalPark() { SceneManager.LoadScene("LocalPark"); }
    public void OnlinePark() { SceneManager.LoadScene("OnlinePark"); }
    public void Progression() { SceneManager.LoadScene("Progression"); }
    public void Credits() { SceneManager.LoadScene("Credits"); }
    public void OnlinePlayground() { SceneManager.LoadScene("OnlinePlayground"); }
    public void OnlineBuilder() { SceneManager.LoadScene("RestrictionMenu"); }
    public void Quit() { Application.Quit(); }

}