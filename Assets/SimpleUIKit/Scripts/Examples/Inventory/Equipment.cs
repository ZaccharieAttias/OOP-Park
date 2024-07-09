using System.Collections.Generic;
using System.Linq;
using Assets.Common.Data;
using Assets.Common.Enums;

namespace Assets.Scripts.Examples.Interface.Elements
{
    public class Equipment : ItemContainer
    {
        public List<ItemSlot> Slots;
		public readonly List<InventoryItem> InventoryItems = new List<InventoryItem>();
        
		public void OnValidate()
        {
            Slots = GetComponentsInChildren<ItemSlot>().ToList();
        }

	    public override void Refresh()
        {
            Reset();

	        foreach (var slot in Slots)
            {
                var item = FindItem(slot);

                slot.gameObject.SetActive(item == null);

                if (item != null)
                {
	                var inventoryItem = CreateInventoryItem(item);
                    var slotDragReceiver = slot.GetComponent<DragReceiver>();

                    inventoryItem.transform.localPosition = slot.transform.localPosition;
                    inventoryItem.transform.SetSiblingIndex(slot.transform.GetSiblingIndex());
					InventoryItems.Add(inventoryItem);
                    inventoryItem.DragReceiver.ColorDropAllowed = slotDragReceiver.ColorDropAllowed;
                    inventoryItem.DragReceiver.ColorDropDenied = slotDragReceiver.ColorDropDenied;
                    inventoryItem.DragReceiver.Group = slotDragReceiver.Group;
                    inventoryItem.DragReceiver.ItemTags = slotDragReceiver.ItemTags;
                    inventoryItem.DragReceiver.ItemTypes = slotDragReceiver.ItemTypes;
                    inventoryItem.DragReceiver.TweenTargets = slotDragReceiver.TweenTargets;
                }
            }
		}

		public void SelectAny()
		{
			InventoryItems[0].OnPress();
			InventoryItems[0].Toggle.isOn = true;
		}

	    public void ToggleItem(Item item)
	    {
			InventoryItems.First(i => i.Item == item).Toggle.isOn = true;
	    }

        private void Reset()
        {
            foreach (var inventoryItem in InventoryItems)
            {
                Destroy(inventoryItem.gameObject);
            }

            InventoryItems.Clear();

            foreach (var slot in Slots)
            {
                slot.busy = false;
            }
		}

        private Item FindItem(ItemSlot slot)
        {
            if (slot.ItemType == ItemType.Shield)
            {
                var twoHandedWeapon = Items.SingleOrDefault(i => i.Type == ItemType.Weapon && i.Tag == ItemTag.Bow);

                if (twoHandedWeapon != null)
                {
                    return twoHandedWeapon;
                }
            }

            var index = Slots.Where(i => i.ItemType == slot.ItemType).ToList().IndexOf(slot);
            var items = Items.Where(i => i.Type == slot.ItemType).ToList();

            return index < items.Count ? items[index] : null;
        }
	}
}