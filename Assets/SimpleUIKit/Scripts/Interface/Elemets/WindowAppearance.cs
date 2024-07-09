using System;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Tweens;
using Assets.Scripts.Interface.Enums;
using UnityEngine;

namespace Assets.Scripts.Interface.Elements
{
    public class WindowAppearance : MonoBehaviour
    {
        public bool Modal;
        public Transform Window;
        public Appearance Appearance;
        public float Duration;
        public AnimationCurve AnimationCurve;

        private Vector3? _origScale;

        public void OnEnable()
        {
            if (Modal) Maximize();
        }

        public void Maximize()
        {
            _origScale ??= Window.localScale;

            Tween.Scale(Window, MaximizeAppearance(), _origScale.GetValueOrDefault(), Duration).AnimationCurve =
                AnimationCurve;
            MakeAlpha(0, 1);
        }

        public void Minimize(Action callback = null)
        {
            if (Appearance == Appearance.None)
            {
                callback?.Invoke();

                return;
            }

            _origScale ??= Window.localScale;

            var tween = Tween.Scale(Window, Window.localScale, MaximizeAppearance(), Duration);

            tween.AnimationCurve = AnimationCurve;
            tween.OnComplete = () => { callback?.Invoke(); };

            MakeAlpha(1, 0);
        }

        public void Hide()
        {
            Minimize(() => this.SetActive(false));
        }

        private Vector3 MaximizeAppearance()
        {
            switch (Appearance)
            {
                case Appearance.Horizontal:
                    return new Vector3(0, _origScale.GetValueOrDefault().y - 0.3f);
                case Appearance.Vertical:
                    return new Vector3(_origScale.GetValueOrDefault().x - 0.3f, 0);
                case Appearance.All:
                    return new Vector3(0, 0);
                case Appearance.None:
                default:
                    return Window.localScale;
            }
        }

        private void MakeAlpha(float from, float to)
        {
            var component = GetComponents<CanvasGroup>().SingleOrDefault() ?? gameObject.AddComponent<CanvasGroup>();
            var tween = Tween.Alpha(component, from, to, Duration);

            tween.AnimationCurve = AnimationCurve;
            tween.OnComplete = () => { Destroy(component); };
        }
    }
}
