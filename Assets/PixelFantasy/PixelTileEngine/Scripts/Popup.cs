using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    public Text Message;
    public TutorialManager TutorialManager;

    public void Show(string message, int duration, string level)
    {
        CanvasGroup.alpha = 1;
        Message.text = message;
        if (TutorialManager) 
        {
            if (level == "C0L1") StartCoroutine(HideTuto(duration));
            else StartCoroutine(Hide(duration));
        }
        else StartCoroutine(Hide(duration));
    }

    private IEnumerator HideTuto(int duration)
    {
        yield return new WaitForSeconds(duration);

        yield return new WaitUntil(() => TutorialManager.hasMoved && TutorialManager.hasJumped);
        
        while (CanvasGroup.alpha > 0)
        {
            CanvasGroup.alpha -= Time.deltaTime;

            yield return null;
        }
        TutorialManager.check++;
    }
    private IEnumerator Hide(int duration)
    {
        yield return new WaitForSeconds(duration);
        
        while (CanvasGroup.alpha > 0)
        {
            CanvasGroup.alpha -= Time.deltaTime;

            yield return null;
        }
    }
}
