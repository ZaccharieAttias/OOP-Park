using System;
using System.Collections;
using System.Globalization;
using Assets.Scripts.Common.Tweens;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Interface
{
	public class PopupSpin : MonoBehaviour
	{
        public GameObject CanvasGroupGO;
		public CanvasGroup CanvasGroup;
        public AnimationCurve AnimationCurve;
        public Text Message;
		public GameObject LoadingButton;
		public GameObject CancelButton;
		public GameObject ConfirmButton;
		public GameObject RetryButton;
        public GameObject BreakButton;
		public InputField Count;
        public bool Break;

        public bool Opened => CanvasGroup.alpha > 0;
        public bool Running => LoadingButton.activeSelf;

        public static PopupSpin Instance;

        private Action _cancel, _confirm, _retry;
        private Action<int> _confirmAsk;
		private string _message;

		public void Awake()
		{
			Instance = this;
		}

        public void BreakNow()
        {
            Break = true;
        }

		public void Run(Action action, string message, bool silent = false, bool breakable = false)
		{
            if (!silent)
            {
				BreakButton.SetActive(breakable);
                Break = false;

                Message.text = message;
                //Message.text = LocalizationManagerAuto.Instance.TryTranslate(message, Message);

                CanvasGroupGO.SetActive(true);
                CanvasGroup.blocksRaycasts = true;
                Tween.Alpha(CanvasGroup, CanvasGroup.alpha, 1, 0.25f).AnimationCurve = AnimationCurve;
                StartSpin();
                StartCoroutine(StartAction(action, 1));
            }
            else
            {
                StartCoroutine(StartAction(action, 0.5f)); 
                ConfirmButton.SetActive(false);
			}
        }
		
        public void RequestConfirmation(string pattern, Action cancel = null, Action confirm = null, Action navigate = null, params object[] args)
        {
            Message.text = _message = string.Join(pattern, args);
            //Message.text = _message = LocalizationManagerAuto.Instance.TryTranslate(pattern, Message, args);

            CanvasGroupGO.SetActive(true);
			CanvasGroup.blocksRaycasts = true;
            Tween.Alpha(CanvasGroup, CanvasGroup.alpha, 1, 0.25f).AnimationCurve = AnimationCurve; ;

            _confirm = confirm;
            PrepareButtons(cancel, confirm, navigate);

            if (cancel != null || confirm != null || navigate != null) return;

            _cancel = () => { };
            CancelButton.SetActive(true);
        }
        
        public void AskCount(string message, Action cancel = null, Action<int> confirm = null, Action retry = null)
        {
            Count.gameObject.SetActive(true);
            _message = message;
            Message.text = _message;
            CanvasGroup.blocksRaycasts = true;
            CanvasGroupGO.SetActive(true);
            Tween.Alpha(CanvasGroup, CanvasGroup.alpha, 1, 0.25f).AnimationCurve = AnimationCurve;

            _cancel = cancel;
            _confirmAsk = confirm;
            _retry = retry;

            LoadingButton.SetActive(false);
            CancelButton.SetActive(cancel != null);
            ConfirmButton.SetActive(confirm != null);
            RetryButton.SetActive(retry != null);

            if (cancel == null && confirm == null && retry == null)
            {
                _cancel = () => { };
                CancelButton.SetActive(true);
            }
        }

        public void Stop(string message, Action cancel = null, Action confirm = null, Action retry = null)
		{
			_confirm = confirm; 

            Message.text = message;
            //Message.text = LocalizationManagerAuto.Instance.TryTranslate(message, Message);

            PrepareButtons(cancel, confirm, retry);

            if (message == null)
            {
				Close();
				return;
            }

            if (cancel != null || confirm != null || retry != null) return;

            _cancel = () => { };
            CancelButton.SetActive(true);
        }

        private void PrepareButtons(Action cancel = null, object confirm = null, Action retry = null)
        {
            _cancel = cancel;
            _retry = retry;

            LoadingButton.SetActive(false);
            CancelButton.SetActive(cancel != null);
            ConfirmButton.SetActive(confirm != null);
            RetryButton.SetActive(retry != null);
        }

		public void OnBackgroundTap()
		{
			if (CancelButton.activeSelf)
			{
				Cancel();
			}
			else if (ConfirmButton.activeSelf)
			{
				Confirm();
			}
		}

		public void Close()
		{
			CanvasGroup.blocksRaycasts = false;
            Count.gameObject.SetActive(false);
			Tween.Alpha(CanvasGroup, CanvasGroup.alpha, 0, 0.25f).AnimationCurve = AnimationCurve; ;
            CanvasGroupGO.SetActive(false);
            CanvasGroup.alpha = 0;
		}

		public void Cancel()
		{
			Close();
			_cancel();
		}

		public void Confirm()
        {
			Close();
			
            if (_confirm == null)
			{
				_confirmAsk(int.Parse(Count.text == "" ? "0" : Count.text, CultureInfo.InvariantCulture));
            }
            else
            {
                _confirm();
            }

            _confirm = null;
        }

        public void Retry()
		{
			StartSpin();
			Message.text = _message;
			_retry();
		}
        
		private void StartSpin()
		{
			LoadingButton.SetActive(true);
			CancelButton.SetActive(false);
			ConfirmButton.SetActive(false);
			RetryButton.SetActive(false);
		}

		private IEnumerator StartAction(Action action, float delay)
		{
			yield return new WaitForSeconds(delay);

			action?.Invoke();
		}
    }
}