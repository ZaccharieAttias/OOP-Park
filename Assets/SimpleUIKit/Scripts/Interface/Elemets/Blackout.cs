using System;
using Assets.Scripts.Common.Tweens;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Interface.Elements
{
    public class Blackout : MonoBehaviour
    {
        public Image Image;
        public float Alpha;
        public float Duration;
	    public AnimationCurve AnimationCurve;

		public bool Opened => Image.raycastTarget = true;
		public static Blackout Instance;

        public void Awake()
        {
            if (Instance == null) Instance = this;
        }

        public void Show()
        {
            Image.raycastTarget = true;
	        Tween.Alpha(Image, Alpha, Duration).AnimationCurve = AnimationCurve;
        }

	    public void Show(Action callback)
	    {
		    Image.raycastTarget = true;

		    var tween = Tween.Alpha(Image, Alpha, Duration);

            tween.AnimationCurve = AnimationCurve;
			tween.OnComplete = callback;
	    }

		public void Hide()
        {
            Image.raycastTarget = false;
            Tween.Alpha(Image, 0, Duration).AnimationCurve = AnimationCurve;
        }

        public void Hide(Action callback)
        {
            Image.raycastTarget = false;

            var tween = Tween.Alpha(Image, 0, Duration);

            tween.AnimationCurve = AnimationCurve;
            tween.OnComplete = callback;
        }

        public void Lock()
        {
            Image.raycastTarget = true;
            Image.color = Color.black;
        }
    }
}