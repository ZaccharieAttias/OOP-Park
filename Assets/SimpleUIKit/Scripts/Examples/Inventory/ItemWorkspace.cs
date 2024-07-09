using Assets.Common.Data;
using UnityEngine;

namespace Assets.Scripts.Examples.Interface.Elements
{
    /// <summary>
    /// Abstract item workspace. It can be shop or player inventory. Items can be managed here (selected, moved and so on).
    /// </summary>
    public abstract class ItemWorkspace : MonoBehaviour
    {
        public ItemInfo ItemInfo;

	    public Item SelectedItem { get; protected set; }

        public abstract void Refresh();

        protected void Reset()
        {
            SelectedItem = null;
            ItemInfo.Reset();
        }

        protected void MoveItem(Item item, ItemContainer from, ItemContainer to)
        {
	        from.Items.Remove(item);
	        to.Items.Add(item);
			Refresh();
	        from.Refresh();
	        to.Refresh();
		}
    }
}