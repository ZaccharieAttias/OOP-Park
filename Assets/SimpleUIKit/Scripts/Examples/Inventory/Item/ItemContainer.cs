using System;
using System.Collections.Generic;
using Assets.Common.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Examples.Interface.Elements
{
    public abstract class ItemContainer : MonoBehaviour
    {
	    public GridLayoutGroup Grid;
	    public ToggleGroup ToggleGroup;
	    public InventoryItem ItemPrefab;

		public List<Item> Items = new List<Item>();

	    public Action<Item> OnItemLeftClick = item => { };
		public Action<Item> OnItemDoubleClick = item => { };
	    public Action<Item> OnItemRightClick = item => { };
	    public Action<Item> OnItemDragStarted = item => { };
	    public Action<Item> OnItemDragCompleted = item => { };
	    
		public abstract void Refresh();

        public void Initialize(ref List<Item> items)
        {
            Items = items;
			Refresh();
        }

	    protected InventoryItem CreateInventoryItem(Item item)
	    {
		    var inventoryItem = Instantiate(ItemPrefab, Grid.transform);

		    inventoryItem.Initialize(item, this);
		    inventoryItem.Toggle.group = ToggleGroup;

		    return inventoryItem;
	    }

        protected InventoryItem UpdateInventoryItem(InventoryItem inventoryItem, Item item)
        {
            inventoryItem.Initialize(item, this);
            inventoryItem.Toggle.group = ToggleGroup;

            return inventoryItem;
        }
	}
}