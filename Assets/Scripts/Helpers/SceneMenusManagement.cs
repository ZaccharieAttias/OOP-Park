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
    public void Finish() { SceneManager.LoadScene("Finish"); }
    public void Quit() { Application.Quit(); }

    public void Chapter0() { SceneManager.LoadScene("ChapterTutorial"); }
    public void C0L1() { SceneManager.LoadScene("C0L1"); }
    public void C0L2() { SceneManager.LoadScene("C0L2"); }
    public void C0L3() { SceneManager.LoadScene("C0L3"); }

    public void Chapter1() { SceneManager.LoadScene("ChapterIneritance"); }
    public void C1L1() { SceneManager.LoadScene("C1L1"); }
    public void C1L2() { SceneManager.LoadScene("C1L2"); }
    public void C1L3() { SceneManager.LoadScene("C1L3"); }
    public void C1L4() { SceneManager.LoadScene("C1L4"); }

    public void Chapter2() { SceneManager.LoadScene("ChapterPolymorphism"); }
    public void C2L1() { SceneManager.LoadScene("C2L1"); }
    public void C2L2() { SceneManager.LoadScene("C2L2"); }
    public void C2L3() { SceneManager.LoadScene("C2L3"); }
    public void C2L4() { SceneManager.LoadScene("C2L4"); }
    public void C2L5() { SceneManager.LoadScene("C2L5"); }
}