using Assets.Scripts.Common.Tweens;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Interface.Elements
{
	public class ToggleSlider : MonoBehaviour
	{
	    public bool Checked;
        public Image Slider;
		public Image SlideArea;
		public Color InactiveColor;
		public Color ActiveColor;
		public float Duration;
		public AnimationCurve AnimationCurve;
		public string Data;
		public Toggle.ToggleEvent OnValueChanged;
		public AudioClip ToggleSound;
	    
		public bool IsOn
		{
			get => Checked;
			set
			{
			    SetWithoutNotify(value);
				OnValueChanged?.Invoke(value);
			}
		}

        public void SetWithoutNotify(bool value)
        {
            var sliderRectTransform = Slider.GetComponent<RectTransform>();

            Checked = value;
            Tween.Color(SlideArea, value ? ActiveColor : InactiveColor, Duration).AnimationCurve = AnimationCurve;
            Tween.PositionAnchored(sliderRectTransform, sliderRectTransform.anchoredPosition, new Vector3((value ? 1 : -1) * (SlideArea.GetComponent<RectTransform>().sizeDelta.x / 2 - sliderRectTransform.sizeDelta.x / 2), sliderRectTransform.anchoredPosition.y), Duration).AnimationCurve = AnimationCurve;
		}

	    public void Switch()
		{
			IsOn = !IsOn;
			Camera.main.GetComponent<AudioSource>().PlayOneShot(ToggleSound);
		}

		public void OnValidate()
        {
            var sliderRectTransform = Slider.GetComponent<RectTransform>();

            SlideArea.color = Checked ? ActiveColor : InactiveColor;
		    Slider.rectTransform.anchoredPosition = new Vector3((Checked ? 1 : -1) * (SlideArea.GetComponent<RectTransform>().sizeDelta.x / 2 - sliderRectTransform.sizeDelta.x / 2), sliderRectTransform.anchoredPosition.y);
        }
	}
}