using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Common.Tweens
{
    [RequireComponent(typeof(MaskableGraphic))]
    public class ColorSpring : TweenBase
    {
        public Color From;
        public Color To;

        private MaskableGraphic _graphic;
        private Color _color;

        public void Awake()
        {
            _graphic = GetComponent<MaskableGraphic>();
            _color = _graphic.color;
        }

        protected override void OnUpdate()
        {
            _graphic.color = From + (To - From) * Sin();
        }

        public void OnDisable()
        {
            _graphic.color = _color;
        }
    }
}