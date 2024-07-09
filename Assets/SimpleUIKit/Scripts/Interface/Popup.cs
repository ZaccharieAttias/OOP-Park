using System.Collections;
using System.Text.RegularExpressions;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Tweens;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Interface
{
	public class Popup : MonoBehaviour
	{
        public GameObject CanvasGroupGO;
		public CanvasGroup CanvasGroup;
        public GameObject PopupTop;
        public Image PopupTopIcon;
        public Text PopupTopMessage;
		public Sprite DefaultIcon;
        public AnimationCurve AnimationCurve;
		public Text Message;
		public float Duration = 3;
        public float DurationTop = 2;
		public static Popup Instance;
        public bool Showing;

		public void Awake()
		{
			Instance = this;
		}

        public void ShowLocalizedMessageTop(Sprite icon, string pattern, params object[] args)
        {
            if (args.Length > 0)
            {
                pattern = string.Format(pattern, args);
            }

            //pattern = LocalizationManagerAuto.Instance.TryTranslate(pattern, PopupTopMessage, args);

            ShowMessageTop(DurationTop, icon == null ? DefaultIcon : icon, pattern);
        }

        public void ShowMessageTop(float duration, Sprite icon, string localizedMessage)
        {
            localizedMessage = Regex.Replace(localizedMessage, @"(\w\.)\.$", "$1");
            duration = Mathf.Max(duration, localizedMessage.Length / 25f);
            icon = icon == null ? DefaultIcon : icon;
            StartCoroutine(Show(duration, localizedMessage, icon));
        }

        public void ShowLocalizedMessage(string pattern, float duration = 3, params object[] args)
        {
            if (args.Length > 0)
            {
                pattern = string.Format(pattern, args);
            }

            //pattern = LocalizationManagerAuto.Instance.TryTranslate(pattern, PopupTopMessage, args);

            ShowMessage(duration, pattern);
        }

        public void ShowMessage(string pattern, float duration = 3, params object[] args)
        {
            if (args.Length > 0)
            {
                pattern = string.Format(pattern, args);
            }

            ShowMessage(duration, string.Format(pattern, args));
		}
        
		private void ShowMessage(float duration, string message)
		{
            message = Regex.Replace(message, @"(\w\.)\.$", "$1");
            duration = Mathf.Max(duration, message.Length / 20f);
            Message.text = message.IsEmpty() ? Message.text : message;
            CanvasGroupGO.SetActive(true);
			CanvasGroup.blocksRaycasts = true;
			Tween.Alpha(CanvasGroup, 0, 1, 0.25f);
			StopAllCoroutines();
			StartCoroutine(FadeOut(new WaitForSeconds(duration)));
		}

		public void Close()
		{
			StopAllCoroutines();
			Hide();
		}

		private IEnumerator FadeOut(WaitForSeconds wait)
		{
			yield return wait;

			Hide();
		}

		private void Hide()
		{
			CanvasGroup.blocksRaycasts = false;
			Tween.Alpha(CanvasGroup, 1, 0, 0.25f);
            CanvasGroupGO.SetActive(false);
		}

        private IEnumerator Show(float duration, string localizedMessage, Sprite icon)
        {
            yield return new WaitWhile(() => Showing);

            Showing = true;

            if (icon != null) PopupTopIcon.sprite = icon;

            PopupTopMessage.text = localizedMessage;

			PopupTop.SetActive(true);
			Tween.Position(PopupTop.transform, new Vector3(0, PopupTop.transform.localPosition.y - 80), duration).AnimationCurve = AnimationCurve;
			
			yield return new WaitForSeconds(duration + 3);

            Hide(duration);
        }

        private void Hide(float duration)
        {
            var tween = Tween.Position(PopupTop.transform, new Vector3(0, PopupTop.transform.localPosition.y + 80), duration);

            tween.AnimationCurve = AnimationCurve;
            tween.OnComplete = () =>
            {
                PopupTop.SetActive(false);
                PopupTopIcon.sprite = DefaultIcon;
                Showing = false;
            };
		}
    }
}