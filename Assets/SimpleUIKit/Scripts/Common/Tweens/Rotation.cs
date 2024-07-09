using UnityEngine;

namespace Assets.Scripts.Common.Tweens
{
    public class Rotation : TweenBase
    {
        public new Axis Axis;

        protected override void OnUpdate()
        {
            var angle = Speed * _time + Period;
            var rotation = transform.localRotation.eulerAngles;

            switch (Axis)
            {
                case Axis.X:
                    transform.localRotation = Quaternion.Euler(angle, rotation.y, rotation.z);
                    break;
                case Axis.Y:
                    transform.localRotation = Quaternion.Euler(rotation.x, angle, rotation.z);
                    break;
                case Axis.Z:
                    transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, angle);
                    break;
            }
        }
    }
}