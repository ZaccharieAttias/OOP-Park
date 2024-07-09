using System.Collections.Generic;
using System.Linq;
using Assets.Common.Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Examples.Interface.Elements
{
    /// <summary>
    /// Used to handle drag & drop operations. It can be the whole inventory area or single item (slot).
    /// </summary>
    public class DragReceiver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// Tween targets will be color-faded when drag events occures.
        /// </summary>
        public List<Image> TweenTargets;

        /// <summary>
        /// If drop is allowed, the spot will be faded with this color.
        /// </summary>
        public Color ColorDropAllowed = new Color(0.5f, 1f, 0.5f);

        /// <summary>
        /// If drop is not allowed, the spot will be faded with this color.
        /// </summary>
        public Color ColorDropDenied = new Color(1f, 0.5f, 0.5f);

        /// <summary>
        /// Group values are used to deny drop items to the same container.
        /// </summary>
        public string Group;

        /// <summary>
        /// Filter items that can be dropped to this drag receiver by type.
        /// </summary>
        public List<ItemType> ItemTypes;

        /// <summary>
        /// Filter items that can be dropped to this drag receiver by tags.
        /// </summary>
        public List<ItemTag> ItemTags;

        /// <summary>
        /// Becomes [true] when drag was started and mouse position is over this drag receiver.
        /// </summary>
        public static bool DropReady;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (InventoryItem.DragTarget == null || InventoryItem.DragTarget.Group == Group) return;

            if (ItemTypes.Any() && !ItemTypes.Contains(InventoryItem.DragTarget.Item.Type))
            {
                Fade(ColorDropDenied);
            }
            else if (ItemTags.Any(i => InventoryItem.DragTarget.Item.Tag != i))
            {
                Fade(ColorDropDenied);
            }
            else
            {
                Fade(ColorDropAllowed);
                DropReady = true;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DropReady = false;
            Fade(Color.white);
        }

        private void Fade(Color color)
        {
            TweenTargets.ForEach(i => i.CrossFadeColor(color, 0.25f, true, false));
        }
    }
}