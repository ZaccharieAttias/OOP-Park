using System.Collections.Generic;
using System.Linq;
using Assets.Common.Data;
using Assets.Common.Enums;
using Assets.Scripts.Common;
using Assets.Scripts.Examples.Interface.Elements;
using Assets.Scripts.Interface.Elements;
using JetBrains.Annotations;
using UnityEngine.UI;

namespace Assets.Scripts.Examples.Interface
{
    /// <summary>
    /// High-level inventory interface.
    /// </summary>
    public class Inventory : ItemWorkspace
    {
        public WindowAppearance Panel;
        public Equipment Equipment;
        public ScrollInventory PlayerInventory; 
        public List<Toggle> TypeFilters;
        [CanBeNull] public Button EquipButton;
        [CanBeNull] public Button RemoveButton;
        [CanBeNull] public Blackout Blackout;

        public void Open(ref List<Item> equipment, ref List<Item> inventory)
        {
            Initialize(ref equipment, ref inventory);

            Panel.SetActive(true);
            Blackout?.Show();
        }

        public void Close()
        {
            Panel.Hide();
            Blackout?.Hide();
        }

        public void Initialize(ref List<Item> equipment, ref List<Item> inventory)
        {
            PlayerInventory.OnItemLeftClick = Equipment.OnItemLeftClick = SelectItem;
            PlayerInventory.OnItemRightClick = Equipment.OnItemRightClick = item => { SelectItem(item); EquipRemove(item); };
            PlayerInventory.OnItemDoubleClick = Equipment.OnItemDoubleClick = EquipRemove; 
            PlayerInventory.OnItemDragStarted = Equipment.OnItemDragStarted = SelectItem;
            PlayerInventory.OnItemDragCompleted = Equipment.OnItemDragCompleted = EquipRemove;
            Equipment.Initialize(ref equipment);
            PlayerInventory.Initialize(ref inventory);
		}
	   
	    public void SelectItem(Item item)
        {
	        ItemInfo.Initialize(SelectedItem = item);

	        if (PlayerInventory.Items.Contains(item))
	        {
		        PlayerInventory.ToggleItem(item);
            }
	        else
            {
                Equipment.ToggleItem(item);
			}
            
            Refresh();
		}

	    private void EquipRemove(Item item)
	    {
			if (item.Type != ItemType.Supplies)
            {
                if (PlayerInventory.Items.Contains(item))
                {
                    Equip();
                }
                else
                {
                    Remove();
                }
            }
        }

	    public void SelectAny()
	    {
			if (SelectedItem != null && PlayerInventory.Items.Contains(SelectedItem)) return;

		    if (PlayerInventory.Items.Count > 0)
		    {
			    PlayerInventory.SelectAny();
		    }
		    else
		    {
			    Equipment.SelectAny();
		    }
	    }

		public void Equip()
        {
            var equipped = Equipment.Items.LastOrDefault(i => i.Type == SelectedItem.Type);

            if (equipped != null)
            {
                AutoRemove(SelectedItem.Type);
            }

            if (SelectedItem.Tag == ItemTag.Bow)
            {
                var shield = Equipment.Items.SingleOrDefault(i => i.Type == ItemType.Shield);

                if (shield != null)
                {
                    MoveItem(shield, Equipment, PlayerInventory);
                }
            }
            else if (SelectedItem.Type == ItemType.Shield)
            {
                var weapon2H = Equipment.Items.SingleOrDefault(i => i.Tag == ItemTag.Bow);

                if (weapon2H != null)
                {
                    MoveItem(weapon2H, Equipment, PlayerInventory);
                }
            }

	        MoveItem(SelectedItem, PlayerInventory, Equipment);
	        SelectItem(SelectedItem);
        }

        public void Remove()
        {
            MoveItem(SelectedItem, Equipment, PlayerInventory);
            SelectItem(SelectedItem);
        }

        public void OnTypeFilter()
        {
            switch (TypeFilters.FindIndex(i => i.isOn))
            {
                case -1: PlayerInventory.SetTypeFilter(null); break;
                case 0: PlayerInventory.SetTypeFilter(ItemType.Weapon); break;
                case 1: PlayerInventory.SetTypeFilter(ItemType.Shield); break;
                case 2: PlayerInventory.SetTypeFilter(ItemType.Armor); break;
                case 3: PlayerInventory.SetTypeFilter(ItemType.Helmet); break;
                case 4: PlayerInventory.SetTypeFilter(ItemType.Supplies); break;
            }
        }

        public override void Refresh()
        {
			if (SelectedItem == null)
            {
                ItemInfo.Reset();
                EquipButton?.SetActive(true);
				RemoveButton?.SetActive(false);
            }
            else
            {
                EquipButton?.SetActive(PlayerInventory.Items != null && PlayerInventory.Items.Contains(SelectedItem));
                RemoveButton?.SetActive(Equipment.Items != null && Equipment.Items.Contains(SelectedItem));
            }
        }
        
        /// <summary>
        /// Automatically removes items if target slot is busy.
        /// </summary>
        private void AutoRemove(ItemType itemType)
        {
	        var remove = Equipment.Items.Where(i => i.Type == itemType).ToList();

			foreach (var item in remove)
	        {
		        Equipment.Items.Remove(item);
		        PlayerInventory.Items.Add(item);
			}
        }
    }
}