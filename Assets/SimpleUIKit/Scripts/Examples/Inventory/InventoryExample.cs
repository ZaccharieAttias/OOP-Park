using System;
using System.Collections.Generic;
using Assets.Common.Data;
using Assets.Common.Enums;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Examples.Inventory
{
    public class InventoryExample : MonoBehaviour
    {
        public Interface.Inventory Inventory;

        public void Open()
        {
            var equipment = GenerateEquipment();
            var inventory = GenerateInventory();

            Inventory.Open(ref equipment, ref inventory);
        }

        private List<Item> GenerateInventory()
        {
            var items = new List<Item>();
            var itemsCount = UnityEngine.Random.Range(4, 10);

            do
            {
                items.Add(GenerateItem((ItemType)UnityEngine.Random.Range(0, (int)ItemType.Supplies + 1)));
            } while (--itemsCount > 0);

            return items;
        }

        private List<Item> GenerateEquipment()
        {
            var items = new List<Item>();

            switch (UnityEngine.Random.Range(0, 99))
            {
                case < 25:
                    items.Add(GenerateItem(ItemType.Helmet));
                    break;
                case < 50:
                    items.Add(GenerateItem(ItemType.Armor));
                    break;
                case < 75:
                    items.Add(GenerateItem(ItemType.Shield));
                    break;
                default:
                    items.Add(GenerateItem(ItemType.Weapon));
                    break;
            }

            return items;
        }

        private Item GenerateItem(ItemType type)
        {
            var name = type switch
            {
                ItemType.Supplies => new List<string> { "Potion", "Scroll", "Coins" }.Random(),
                ItemType.Armor => new List<string> { "Light Armor", "Robe Armor", "Heavy Armor" }.Random(),
                ItemType.Helmet => type.ToString(),
                ItemType.Shield => type.ToString(),
                _ => new List<string> { "Axe", "Sword", "Mace", "Dagger", "Wand", "Bow" }.Random()
            };

            var item = new Item()
            {
                Id = name,
                Rarity = (ItemRarity)UnityEngine.Random.Range(0, (int)ItemRarity.Epic + 1),
                Name = name,
                Type = type,
                Tag = MakeRandomTag(name),
                Power = UnityEngine.Random.Range(0, 100),
                Weight = UnityEngine.Random.Range(0, 100),
                New = CRandom.Chance(20),
                Count = type == ItemType.Supplies ? UnityEngine.Random.Range(0, 10) : 1
            };

            item.Description = $"Some {item.Rarity} {name}";

            return item;
        }

        private ItemTag MakeRandomTag(string name)
        {
            switch (name)
            {
                case "Potion":
                case "Scroll":
                case "Coins":
                    return ItemTag.Consumable;
                case "Light Armor":
                    return ItemTag.Light;
                case "Robe Armor":
                    return ItemTag.Robe;
                case "Helmet":
                case "Heavy Armor":
                case "Shield":
                    return ItemTag.Heavy;
                default:
                    return Enum.Parse<ItemTag>(name);
            }
        }
    }
}