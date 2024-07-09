using UnityEngine;

namespace Assets.Scripts.Interface.Elements
{
    public class SimpleDropdown : MonoBehaviour
    {
        public float Speed;
        public Vector2 FromSpacing;
        public Vector2 ToSpacing;
        public DirectionType Direction;
        public Vector2 GridCellSize;

        public enum DirectionType
        {
            Up,
            Down,
            UpDown
        }
    }
}
