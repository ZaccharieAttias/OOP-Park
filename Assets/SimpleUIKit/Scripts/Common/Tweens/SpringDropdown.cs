using Assets.Scripts.Interface.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Common.Tweens
{
    public class SpringDropdown : MonoBehaviour
    {
        public GridLayoutGroup GridLayoutGroup;
        public GridSpring GridSpring;
        public Button GridFrame;
        public SimpleDropdown SimpleDropdown;

        private int _selectedItemIndexOrig;
        private Transform _selectedItem;

        public void OnEnable()
        {
            var children = GridSpring.GetComponentsInChildren<Button>();

            foreach (var child in children)
            {
                child.onClick.RemoveListener(() => ItemOnClick(child.transform));
                child.onClick.AddListener(() => ItemOnClick(child.transform));
            }

            GridLayoutGroup.cellSize = SimpleDropdown.GridCellSize;
            GridFrame.GetComponent<RectTransform>().sizeDelta = new Vector2(
                SimpleDropdown.GridCellSize.x + SimpleDropdown.FromSpacing.x,
                (SimpleDropdown.GridCellSize.y + SimpleDropdown.FromSpacing.y) * children.Length);
        }

        public void ItemOnClick(Transform item)
        {
            if (GridSpring.enabled) return;

            if (GridFrame.IsActive())
            {
                if (_selectedItem != null)
                {
                    _selectedItem.SetSiblingIndex(_selectedItemIndexOrig);
                }

                GridSpring.To = SimpleDropdown.ToSpacing;
                GridSpring.From = SimpleDropdown.FromSpacing;
            }
            else
            {
                _selectedItem = item;
                _selectedItemIndexOrig = item.transform.GetSiblingIndex();
                item.SetAsLastSibling();

                GridSpring.To = SimpleDropdown.FromSpacing;
                GridSpring.From = SimpleDropdown.ToSpacing;
            }

            GridSpring.Speed = SimpleDropdown.Speed;

            GridLayoutGroup.childAlignment = SimpleDropdown.Direction switch
            {
                SimpleDropdown.DirectionType.Up => TextAnchor.LowerCenter,
                SimpleDropdown.DirectionType.Down => TextAnchor.UpperCenter,
                _ => TextAnchor.MiddleCenter
            };

            GridSpring.enabled = true;
            GridFrame.SetActive(!GridFrame.IsActive());
        }
    }
}
