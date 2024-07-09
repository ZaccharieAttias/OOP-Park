using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Interface.Elements
{
	public class CustomSlider : MonoBehaviour
    {
		public Slider Slider;
		public InputField InputField;
		public bool ZeroLocalized;
        public bool Lazy;

		public int Value
        {
            get => Mathf.RoundToInt(Slider.value);
            set { Slider.SetValueWithoutNotify(value); SetInputField(value); }
        }

        public float NormalizedValue => Slider.value / Slider.maxValue;
        public bool Interactable { set => Slider.interactable = InputField.interactable = value; }

        [Serializable]
		public class OnChangedEvent : UnityEvent<int>
		{
		}

		public OnChangedEvent OnChanged;

        public void OnEnable()
        {
            InputField.contentType = Slider.minValue >= 0 ? InputField.ContentType.IntegerNumber : InputField.ContentType.Standard; // TODO: Workaround for "-" symbol.
        }

        public void Initialize(int value, int min = 0, int max = 1)
        {
            _mute = Time.frameCount;
            Slider.minValue = min;
			Slider.maxValue = max;
            Slider.value = value;
            SetInputField(value);
        }

        public void Set(float value)
        {
            Slider.SetValueWithoutNotify(value);
            SetInputField(Mathf.RoundToInt(value));
        }

        private int _mute = -1;

        public void OnValueChanged(float value)
        {
            var valueInt = Mathf.RoundToInt(value);

			SetInputField(valueInt);

            if (Time.frameCount != _mute)
            {
                if (Lazy && Input.GetMouseButton(0))
                {
                    _mouseUpEvent = () => OnChanged.Invoke(valueInt);
                }
                else
                {
                    OnChanged.Invoke(valueInt);
                }
            }
		}

		public void OnValueChanged(string text)
		{
            var negative = text.StartsWith("-");

            text = Regex.Replace(text, @"[^\d]+", "");

            if (text == "")
            {
                InputField.SetTextWithoutNotify(negative ? "-" : "");
                return;
            }

            var value = int.Parse(text);

            if (negative) value *= -1;
            
            if (value > Slider.maxValue) value = (int) Slider.maxValue;

            SetInputField(value);
            Slider.SetValueWithoutNotify(value);

            if (value >= Slider.minValue && value <= Slider.maxValue && !Lazy)
            {
                OnChanged.Invoke(value);
            }
        }

        public void OnEndEdit(string text)
        {
            if (int.TryParse(text, out var value))
            {
                if (value < Slider.minValue) value = (int)Slider.minValue;
                if (value > Slider.maxValue) value = (int)Slider.maxValue;

                if (value != int.Parse(text))
                {
                    Slider.SetValueWithoutNotify(value);
                    SetInputField(value);
                }
            }
            else
            {
                value = Slider.minValue >= 0 ? (int) Slider.minValue : 0;

                Slider.SetValueWithoutNotify(value);
                SetInputField(value);
            }

            if (Lazy)
            {
                OnChanged.Invoke(value);
            }
        }

		private void SetInputField(int value)
		{
			if (value == 0 && ZeroLocalized)
			{
				InputField.SetTextWithoutNotify(null);
			}
			else
			{
                InputField.SetTextWithoutNotify((Slider.minValue < 0 && value > 0 ? "+" : "") + value);
                InputField.caretPosition = InputField.text.Length;
            }
		}

        private Action _mouseUpEvent;

        public void Update()
        {
            if (Input.GetMouseButtonUp(0) && _mouseUpEvent != null)
            {
                _mouseUpEvent();
                _mouseUpEvent = null;
            }
        }
    }
}