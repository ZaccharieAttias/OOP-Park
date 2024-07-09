using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Assets.Common.Enums;
using Newtonsoft.Json;

namespace Assets.Common.Data
{
    [Serializable]
	public class Item : ICloneable
    {
        public string Id;
        public string InstanceId;
        public ItemRarity Rarity;
		public ItemType Type;
        public ItemTag Tag;
        public string Name;
        public string Description;
        public int Power;
        public int Weight;
        public bool New;
        public int Count;
		
		public Item()
		{

        }

		public bool Equals(Item other)
		{
			return Id == other.Id && Id == other.Id && InstanceId == other.InstanceId && Count == other.Count;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public object Clone()
		{
			return MemberwiseClone();
		}
	}

	public class ItemComparer : IEqualityComparer<Item>
	{
		public bool Equals(Item a, Item b)
		{
			if (ReferenceEquals(a, b)) return true;

			return a != null && b != null && a.Equals(b);
		}

		public int GetHashCode(Item obj)
		{
			return obj.GetHashCode();
		}
	}
}