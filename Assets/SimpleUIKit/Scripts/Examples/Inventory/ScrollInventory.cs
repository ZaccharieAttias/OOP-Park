using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Common.Data;
using Assets.Common.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Examples.Interface.Elements
{
    /// <summary>
    /// Scrollable item container that can display item list. Automatic vertical scrolling.
    /// </summary>
    [RequireComponent(typeof(DragReceiver))]
    public class ScrollInventory : ItemContainer
    {
        public ScrollRect ScrollRect;
        public GameObject CellPrefab;
	    public bool AddEmptyCells = true;

		private Action<List<Item>> _sorting;
        private List<ItemRarity> _filterByRarity;
        private List<ItemType> _filterByType;
	    private ItemAttributeId? _filterByAttribute;
        public Dictionary<Item, InventoryItem> InventoryItems = new Dictionary<Item, InventoryItem>(); // Reusing instances to reduce Instantiate() calls.
	    private List<GameObject> _emptyCells = new List<GameObject>();
	    private bool _initialized;

	    public Action<List<InventoryItem>> OnRefresh;

	    public void SetSorting(Action<List<Item>> sorting)
	    {
		    _sorting = sorting;
		    Refresh(force: true);
		}
		
        public void SetRarityFilter(List<ItemRarity> rarities)
        {
            if (rarities == null && _filterByRarity == null) return;
            if (rarities != null && _filterByRarity != null && rarities.SequenceEqual(_filterByRarity)) return;

            _filterByRarity = rarities;
            Refresh(force: true);
        }

        public void SetTypeFilter(ItemType type)		// TODO: filter by Auctioned (locked)
        {
            SetTypeFilter(new List<ItemType> { type });
        }

        public void SetTypeFilter(List<ItemType> types)
	    {
	        if (types == null && _filterByType == null) return;
	        if (types != null && _filterByType != null && types.SequenceEqual(_filterByType)) return;

	        _filterByType = types;
		    Refresh(force: true);
		}

	    public void SetAttributeFilter(ItemAttributeId? attributeId)
	    {
		    if (attributeId == _filterByAttribute) return;

		    _filterByAttribute = attributeId;
		    Refresh(force: true);
	    }
		
	    public override void Refresh()
        {
			Refresh(force: false);
        }

	    public void SelectAny()
	    {
			if (InventoryItems.Count == 0) return;

			var target = InventoryItems.Values.First();
			
		    target.OnPress();
		    target.Toggle.isOn = true;
		}

	    public void ToggleItem(Item item)
	    {
		    if (InventoryItems.ContainsKey(item))
		    {
			    InventoryItems[item].Toggle.isOn = true;
			}
		}

		private void Refresh(bool force)
	    {
	        if (Items == null || !force && _initialized && Items.SequenceEqual(InventoryItems.Keys)) return;

            var inventoryItems = new Dictionary<Item, InventoryItem>();
			var emptyCells = new List<GameObject>();
		    var ids = Items.Select(i => i.Id).ToList();
			var items = Items.OrderBy(i => i.Type).ThenBy(i => ids.IndexOf(i.Id)).ToList();
			var dragReceiver = GetComponent<DragReceiver>();

		    _sorting?.Invoke(items);
			
            if (_filterByRarity != null)
	        {
	            items.RemoveAll(i => !_filterByRarity.Contains(i.Rarity));
            }

			if (_filterByType != null)
			{
			    items.RemoveAll(i => !_filterByType.Contains(i.Type));
            }
			
			foreach (var item in items)
			{
				InventoryItem inventoryItem;

				if (InventoryItems.ContainsKey(item))
				{
					inventoryItem = InventoryItems[item];
                    inventoryItem = UpdateInventoryItem(inventoryItem, item);
					inventoryItem.transform.SetAsLastSibling();
					inventoryItems.Add(item, inventoryItem);
					InventoryItems.Remove(item);
				}
				else
				{
					inventoryItem = CreateInventoryItem(item);
					inventoryItem.Group = dragReceiver.Group;
					inventoryItems.Add(item, inventoryItem);
				}
            }

			if (AddEmptyCells)
			{
				var columns = 0;
				var rows = 0;

				switch (Grid.constraint)
				{
					case GridLayoutGroup.Constraint.FixedColumnCount:
						{
							var height = Mathf.FloorToInt((ScrollRect.viewport.rect.height - Grid.padding.top - Grid.padding.bottom) / (Grid.cellSize.y + Grid.spacing.y));

							columns = Grid.constraintCount;
							rows = Mathf.Max(height, Mathf.CeilToInt((float) items.Count / columns));

							break;
						}
					case GridLayoutGroup.Constraint.FixedRowCount:
						{
							var width = Mathf.FloorToInt((ScrollRect.viewport.rect.width - Grid.padding.top - Grid.padding.bottom) / (Grid.cellSize.x + Grid.spacing.x));

							rows = Grid.constraintCount;
							columns = Mathf.Max(width, Mathf.CeilToInt((float) items.Count / rows));

							break;
						}
				}

				for (var i = items.Count; i < columns * rows; i++)
				{
					var existing = _emptyCells.LastOrDefault();

					if (existing != null)
					{
						existing.transform.SetAsLastSibling();
						emptyCells.Add(existing);
						_emptyCells.Remove(existing);
					}
					else
					{
						emptyCells.Add(Instantiate(CellPrefab, Grid.transform));
					}
				}
			}

			foreach (var item in InventoryItems)
			{
				Destroy(item.Value.gameObject);
			}

			foreach (var instance in _emptyCells)
			{
				Destroy(instance);
			}

			InventoryItems = inventoryItems;
			_emptyCells = emptyCells;
		    _initialized = true;

		    OnRefresh?.Invoke(InventoryItems.Values.ToList());
	    }
    }
}