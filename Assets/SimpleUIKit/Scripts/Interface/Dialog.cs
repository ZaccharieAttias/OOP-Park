using Assets.Scripts.Common;
using Assets.Scripts.Common.Tweens;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Interface.Dialog
{
    public class Dialog : MonoBehaviour
    {
        public CanvasGroup CanvasGroup;
        public Text Message;

        public void Show(string message = "")
        {
            Message.text = message.IsEmpty() ? Message.text : message;
            //Message.text = LocalizationManagerAuto.Instance.TryTranslate(message ?? Message.text, Message);

            CanvasGroup.SetActive(true);
            Tween.Alpha(CanvasGroup, CanvasGroup.alpha, 1, 0.25f);
        }

        public void Hide()
        {
            Tween.Alpha(CanvasGroup, CanvasGroup.alpha, 0, 0.25f);
            CanvasGroup.SetActive(false);
        }
    }
}
