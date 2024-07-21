using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    public Text Message;
    public TutorialManager TutorialManager;

    public void Show(string message, int duration)
    {
        CanvasGroup.alpha = 1;
        Message.text = message;
        StartCoroutine(Hide(duration));
    }

    private IEnumerator Hide(int duration)
    {
        yield return new WaitForSeconds(duration);
        // attendre que hasMoved et hasJumped soit vrai
        yield return new WaitUntil(() => TutorialManager.hasMoved && TutorialManager.hasJumped);
        
        while (CanvasGroup.alpha > 0)
        {
            CanvasGroup.alpha -= Time.deltaTime;

            yield return null;
        }
        TutorialManager.check++;
    }
}
