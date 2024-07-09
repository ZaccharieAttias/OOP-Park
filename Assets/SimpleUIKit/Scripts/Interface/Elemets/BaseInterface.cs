using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Interface.Elements;
using UnityEngine;

namespace Assets.Scripts.Interface
{
	public class BaseInterface : MonoBehaviour
	{
		public GameObject Panel;
        public Blackout Blackout;
        public bool Modal;
        
		public bool Opened => Panel.activeSelf;

		public static readonly List<BaseInterface> Current = new();
        public static BaseInterface Previous;
        
        private WindowAppearance _windowAppearance;

        [Serializable]
        [Flags]
        public enum Arg
        {
            SomeArg0,
            SomeArg1,
            SomeArg2
        }

        public void Start()
        {
            TryGetComponent<WindowAppearance>(out _windowAppearance);
        }

        public void OnValidate()
		{
			Panel ??= transform.Find("Panel").gameObject;
		}

		public void Open()
		{
            if (Opened && Current.Contains(this)) return;

            ClosePrevious();

            if (Blackout != null)
            {
                Blackout.Show(() =>
                {
                    Show();
                    OnOpen();
                    Activate();
                });
            }
            else
            {
                Show();
                OnOpen();
                Activate();
            }
        }

        public void Open(string arg)
        {
            if (Opened && Current.Contains(this)) return;

            ClosePrevious();

            Blackout.Instance.Show(() =>
            {
                Show();

                if (Enum.TryParse<Arg>(arg, true, out var _action))
                {
                    OnOpen(arg);
                }
                else
                {
                    OnOpen(arg);
                }

                Activate();
            });
        }

        public void Open(uint i)
        {
            if (Opened && Current.Contains(this)) return;

            ClosePrevious();

            Blackout.Instance.Show(() =>
            {
                Show();
                OnOpen(i);
                Activate();
            });
        }

        private void ClosePrevious()
        {
            if (Modal) return;

            foreach (var window in Current.Where(i => !i.Modal).ToList())
            {
                if (window._windowAppearance != null)
                {
                    window._windowAppearance.Minimize(() => window.Close());
                }
                else
                {
                    window.Close();
                }
            }
        }
        
        public void Close()
        {
            if (!Opened) return;
            
            Panel.SetActive(false);
            Current.Remove(this);
            Previous = this;
            OnClose();
        }

        private void Activate()
        {
            Panel.SetActive(true);
            Current.Add(this);
        }

        private void Show()
        {
            if (_windowAppearance != null)
            {
                _windowAppearance.Maximize();
            }

            Blackout?.Hide();
        }

        protected virtual void OnOpen()
		{
		}
        
        protected virtual void OnOpen(Arg action)
        {
        }

        protected virtual void OnOpen(string arg)
        {
        }

        protected virtual void OnOpen(uint i)
        {
        }

        protected virtual void OnClose()
		{
		}

		public void OnDestroy()
		{
			if (Current.Contains(this)) Current.Remove(this);
		}
    }
}