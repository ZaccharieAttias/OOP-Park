using System.Collections.Generic;
using System.Linq;
using Assets.Common.Data;
using Assets.Common.Enums;
using Assets.Scripts.Common;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Examples.Interface.Elements
{
    /// <summary>
    /// Represents inventory item and handles drag & drop operations.
    /// </summary>
    public class InventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Image Icon;
        public Image Frame;
        public Text Count;
        public Toggle Toggle;
        public GameObject NewTag;
        public GameObject Equipped;
        public string Group;
        public Item Item;
        public Image RarityIcon;
        public Sprite DefaultItemIcon;
        public List<Sprite> RaritySprites;
        public List<Sprite> IconCollectionExample;
        [CanBeNull] public DragReceiver DragReceiver;

        private ItemContainer _container;
        private GameObject _phantom;
        private RectTransform _rect;
        private float _clickTime;

        public static InventoryItem DragTarget;

        public void Initialize(Item item, ItemContainer container = null)
        {
            Item = item;
            _container = container;
            Icon.sprite = GetIcon(Item.Id);

            if (NewTag != null)
            {
                NewTag.SetActive(Item.New);
            }

            RarityIcon.sprite = RaritySprites[(int)Item.Rarity];
            
            if (item.Type == ItemType.Supplies)
            {
                Count.SetActive(true);
                Count.text = $"{Item.Count}";
            }
        }

        private Sprite GetIcon(string itemName)
        {
            return IconCollectionExample.FirstOrDefault(i => i.name == itemName);
        }

        /// <summary>
        /// Called from button script.
        /// </summary>
        public void OnPress()
        {
            if (_container != null) _container.OnItemLeftClick(Item);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (_container != null) _container.OnItemLeftClick(Item);

                if (Mathf.Abs(eventData.clickTime - _clickTime) < 0.5f) // If double click
                {
                    if (_container != null) _container.OnItemDoubleClick(Item);
                }

                _clickTime = eventData.clickTime;
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (_container != null) _container.OnItemRightClick(Item);
            }

            if (NewTag != null)
            {
                NewTag.SetActive(Item.New = false);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (DragTarget != null) return;

            var canvas = FindInParents<Canvas>(gameObject);

            _phantom = Instantiate(gameObject);

            var phantomRectTransform = _phantom.GetComponent<RectTransform>();
            var rectTransform = GetComponent<RectTransform>();

            _phantom.transform.SetParent(canvas.transform, true);
            _phantom.transform.SetAsLastSibling();
            phantomRectTransform.sizeDelta = rectTransform.sizeDelta;
            phantomRectTransform.localScale = rectTransform.localScale;
            _phantom.GetComponent<CanvasGroup>().blocksRaycasts = false;
            _rect = canvas.GetComponent<RectTransform>();
            SetDraggedPosition(eventData);
            DragTarget = this;

            if (_container != null) _container.OnItemDragStarted(Item);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (DragTarget == null) return;

            SetDraggedPosition(eventData);
        }

        public void Refresh()
        {
            Count.SetActive(Item.Count > 0);
            Count.text = (Item.Count + 1).ToString();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (DragTarget == null) return;

            if (DragReceiver.DropReady)
            {
                if (_container != null) _container.OnItemDragCompleted(Item);
            }

            DragTarget = null;
            DragReceiver.DropReady = false;
            Destroy(_phantom, 0.25f);

            foreach (var graphic in _phantom.GetComponentsInChildren<Graphic>())
            {
                graphic.CrossFadeAlpha(0f, 0.25f, true);
            }
        }

        private void SetDraggedPosition(PointerEventData data)
        {
            if (!RectTransformUtility.ScreenPointToWorldPointInRectangle(_rect, data.position, data.pressEventCamera, out var mouse)) return;

            var rect = _phantom.GetComponent<RectTransform>();

            rect.position = mouse;
            rect.rotation = _rect.rotation;
        }

        private static T FindInParents<T>(GameObject go) where T : Component
        {
            if (go == null) return null;

            var comp = go.GetComponent<T>();

            if (comp != null) return comp;

            var t = go.transform.parent;

            while (t != null && comp == null)
            {
                comp = t.gameObject.GetComponent<T>();
                t = t.parent;
            }

            return comp;
        }

    }
}