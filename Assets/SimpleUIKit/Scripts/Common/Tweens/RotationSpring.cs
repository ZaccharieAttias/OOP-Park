using UnityEngine;

namespace Assets.Scripts.Common.Tweens
{
    public class RotationSpring : TweenBase
    {
        public new Axis Axis;
        public float From;
        public float To;
        public float Dumping;

        private float _rotation;

        public override void OnEnable()
        {
            base.OnEnable();

            var rotation = transform.localEulerAngles;

            switch (Axis)
            {
                case Axis.X: _rotation = rotation.x; break;
                case Axis.Y: _rotation = rotation.y; break;
                case Axis.Z: _rotation = rotation.z; break;
            }
        }

        public void OnDisable()
        {
            transform.localEulerAngles = GetRotation(_rotation);
        }

        protected override void OnUpdate()
        {
            var _amplitude = _rotation + From + (To - From) * Sin() - Dumping * Time.deltaTime;

            transform.localEulerAngles = GetRotation(_amplitude);

            if (_amplitude <= 0)
            {
                enabled = false;
            }
        }

        private Vector3 GetRotation(float angle)
        {
            var rotation = transform.localEulerAngles;

            switch (Axis)
            {
                case Axis.X: rotation.x = angle; break;
                case Axis.Y: rotation.y = angle; break;
                case Axis.Z: rotation.z = angle; break;
            }

            return rotation;
        }
    }
}