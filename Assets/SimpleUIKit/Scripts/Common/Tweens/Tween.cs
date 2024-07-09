using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

namespace Assets.Scripts.Common.Tweens
{
    public class Tween : MonoBehaviour
    {
        public AnimationCurve AnimationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
        public Action OnComplete;

        private float _timeout;
        private Action _update;
        private Action _complete;
	    private int _type;
        
		public static Tween Color(Graphic target, Color from, Color to, float duration)
        {
            target.color = from;
            
            var component = target.GetComponents<Tween>().SingleOrDefault(i => i._type == 0) ?? target.gameObject.AddComponent<Tween>();
            var time = Time.time;

            component._update = () => { target.color = from + (to - from) * component.AnimationCurve.Evaluate((Time.time - time) / duration); };
            component._complete = () => { target.color = to; };
            component._timeout = Time.time + duration;
            component._type = 0;
            component.enabled = true;

            return component;
        }

        public static Tween Color(Graphic target, Color to, float duration)
        {
            return Color(target, target.color, to, duration);
        }

        public static Tween Alpha(Graphic target, float from, float to, float duration)
        {
            return Color(target, new Color(target.color.r, target.color.g, target.color.b, from), new Color(target.color.r, target.color.g, target.color.b, to), duration);
        }

        public static Tween Alpha(Graphic target, float to, float duration)
        {
            return Alpha(target, target.color.a, to, duration);
        }

        public static Tween Color(SpriteRenderer target, Color from, Color to, float duration)
        {
            target.color = from;

            var component = target.GetComponent<Tween>() ?? target.gameObject.AddComponent<Tween>();
            var time = Time.time;

            component._update = () => { target.color = from + (to - from) * component.AnimationCurve.Evaluate((Time.time - time) / duration); };
            component._complete = () => { target.color = to; };
            component._timeout = Time.time + duration;
            component.enabled = true;
            component.OnComplete = null;

            return component;
        }

        public static Tween Color(SpriteRenderer target, Color to, float duration)
        {
            return Color(target, target.color, to, duration);
        }

        public static Tween Alpha(SpriteRenderer target, float from, float to, float duration)
        {
            return Color(target, new Color(target.color.r, target.color.g, target.color.b, from), new Color(target.color.r, target.color.g, target.color.b, to), duration);
        }

        public static Tween Alpha(SpriteRenderer target, float to, float duration)
        {
            return Alpha(target, target.color.a, to, duration);
        }

        public static Tween Alpha(CanvasGroup target, float from, float to, float duration)
        {
            target.alpha = from;

            var component = target.GetComponents<Tween>().SingleOrDefault(i => i._type == 1) ?? target.gameObject.AddComponent<Tween>();
            var time = Time.time;

            component._update = () => { target.alpha = from + (to - from) * component.AnimationCurve.Evaluate((Time.time - time) / duration); };
            component._complete = () => { target.alpha = to; };
            component._timeout = Time.time + duration;
            component._type = 1;
            component.enabled = true;
            component.OnComplete = null;

            return component;
        }

        public static Tween Alpha(CanvasGroup target, float to, float duration)
        {
            return Alpha(target, target.alpha, to, duration);
        }
        
        public static Tween PositionAnchored(RectTransform target, Vector2 from, Vector2 to, float duration)
        {
            var component = target.GetComponents<Tween>().SingleOrDefault(i => i._type == 2) ?? target.gameObject.AddComponent<Tween>();
            var time = Time.time;
            
            component._update = () => { target.anchoredPosition = from + (to - from) * component.AnimationCurve.Evaluate((Time.time - time) / duration); };
            component._complete = () => { target.anchoredPosition = to; };
            
            component._timeout = Time.time + duration;
            component._type = 2;
            component.enabled = true;
            component.OnComplete = null;

            return component;
        }

        public static Tween Position(Transform target, Vector3 from, Vector3 to, float duration, bool local = true)
        {
            if (local)
            {
                target.localPosition = from;
            }
            else
            {
                target.position = from;
            }

            var component = target.GetComponents<Tween>().SingleOrDefault(i => i._type == 2) ?? target.gameObject.AddComponent<Tween>();
            var time = Time.time;

            if (local)
            {
                component._update = () => { target.localPosition = from + (to - from) * component.AnimationCurve.Evaluate((Time.time - time) / duration); };
                component._complete = () => { target.localPosition = to; };
            }
            else
            {
                component._update = () => { target.position = from + (to - from) * component.AnimationCurve.Evaluate((Time.time - time) / duration); };
                component._complete = () => { target.position = to; };
            }

            component._timeout = Time.time + duration;
            component._type = 2;
            component.enabled = true;
            component.OnComplete = null;

            return component;
        }

        public static Tween Position(Transform target, Vector3 to, float duration, bool local = true)
        {
            return Position(target, target.localPosition, to, duration, local);
        }

        public static Tween Position(RectTransform target, Vector3 to, float duration, bool local = true)
        {
            return Position(target, target.anchoredPosition, to, duration, local);
        }

        public static Tween Scale(Transform target, Vector3 from, Vector3 to, float duration)
        {
            target.localScale = from;

            var component = target.GetComponents<Tween>().SingleOrDefault(i => i._type == 3) ?? target.gameObject.AddComponent<Tween>();
            var time = Time.time;

            component._update = () => { target.localScale = from + (to - from) * component.AnimationCurve.Evaluate((Time.time - time) / duration); };
            component._complete = () => { target.localScale = to; };

            component._timeout = Time.time + duration;
            component._type = 3;
            component.enabled = true;
            component.OnComplete = null;

            return component;
        }

        public static Tween Scale(Transform target, Vector3 to, float duration, bool local = true)
        {
            return Scale(target, target.localScale, to, duration);
        }

        public static Tween Scale(Transform target, float to, float duration, bool local = true)
        {
            return Scale(target, target.localScale, to * Vector3.one, duration);
        }

        public static Tween Scale(Transform target, float from, float to, float duration, bool local = true)
        {
            return Scale(target, from * Vector3.one, to * Vector3.one, duration);
        }

        public static Tween FillAmount(Image target, float from, float to, float duration)
        {
            target.fillAmount = from;

            var component = target.GetComponents<Tween>().SingleOrDefault(i => i._type == 4) ?? target.gameObject.AddComponent<Tween>();
            var time = Time.time;

            component._update = () => { target.fillAmount = from + (to - from) * component.AnimationCurve.Evaluate((Time.time - time) / duration); };
            component._complete = () => { target.fillAmount = to; };

            component._timeout = Time.time + duration;
            component._type = 4;
            component.enabled = true;
            component.OnComplete = null;

            return component;
        }

        public static Tween FillAmount(Image target, float to, float duration, bool local = true)
        {
            return FillAmount(target, target.fillAmount, to, duration);
        }

        public static Tween Spacing(GridLayoutGroup target, Vector3 from, Vector3 to, float duration)
        {
            target.spacing = from;

            var component = target.GetComponents<Tween>().SingleOrDefault(i => i._type == 3) ?? target.gameObject.AddComponent<Tween>();
            var time = Time.time;

            component._update = () => { target.spacing = from + (to - from) * component.AnimationCurve.Evaluate((Time.time - time) / duration); };
            component._complete = () => { target.spacing = to; };

            component._timeout = Time.time + duration;
            component._type = 5;
            component.enabled = true;
            component.OnComplete = null;

            return component;
        }

        public static Tween Spacing(GridLayoutGroup target, Vector3 to, float duration)
        {
            return Spacing(target, target.spacing, to, duration);
        }

        public void Update()
        {
            if (Time.time < _timeout)
            {
                _update();
            }
            else
            {
                _complete?.Invoke();
                _type = -1;
                Destroy(this);

                if (OnComplete != null)
                {
                    OnComplete();
                }
            }
        }
    }
}