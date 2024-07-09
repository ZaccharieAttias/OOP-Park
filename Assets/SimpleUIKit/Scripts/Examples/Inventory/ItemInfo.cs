using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Common.Data;
using UnityEngine;
using UnityEngine.UI;
using Assets.Common.Enums;

namespace Assets.Scripts.Examples.Interface.Elements
{
    /// <summary>
    /// Represents item when it was selected. Displays icon, name, price and properties.
    /// </summary>
    public class ItemInfo : MonoBehaviour
    {
        public Text Name;
        public Image Rarity;
        public Image Icon;
        public Text AttributeNames;
	    public Text AttributeValues;
        public Text Description;
        public Sprite DefaultItemIcon;
        public List<Sprite> RaritySprites;
        public List<Sprite> IconCollectionExample;
        
        public void Reset()
        {
	        Name.text = "Nothing Selected";
			AttributeNames.text = AttributeValues.text = null;
	        Icon.sprite = DefaultItemIcon;
        }

        public void Initialize(Item item)
        {
            if (item == null)
            {
                Reset();

                return;
            }

            Rarity.sprite = RaritySprites[(int)item.Rarity];
            Icon.sprite = GetIcon(item.Id);
            Name.text = item.Name;
            Description.text = item.Description;

            var equipmentClasses = new List<ItemTag>
                { ItemTag.Heavy, ItemTag.Light, ItemTag.Robe, ItemTag.Consumable, ItemTag.UnConsumable, ItemTag.Axe };
            var equipmentMeta = new List<ItemAttributeId>
            {
                ItemAttributeId.StrengthReq, ItemAttributeId.IntelligenceReq, ItemAttributeId.DexterityReq,
                ItemAttributeId.UnblockableDamage, ItemAttributeId.CriticalChance, ItemAttributeId.ConstitutionUp,
                ItemAttributeId.DexterityUp, ItemAttributeId.IntelligenceUp, ItemAttributeId.StrengthReq
            };

            var dict = new Dictionary<string, string>();
            
            dict.Add("Rarity", item.Rarity.ToString());
            dict.Add("Type", item.Type.ToString());
            dict.Add("Class", item.Tag.ToString());

            switch (item.Type)
            {
                case ItemType.Weapon:
                    dict.Add("Damage", $"{item.Power}");
                    break;
                case ItemType.Armor:
                case ItemType.Shield:
                case ItemType.Helmet:
                    dict.Add("Defense", $"{item.Power}");
                    break;
                case ItemType.Supplies:
                default:
                    dict.Add("Power", $"{item.Power}");
                    break;
            }

            dict.Add("Weight", item.Weight.ToString()); 
            
            AttributeNames.text = string.Join(Environment.NewLine, dict.Keys);
            AttributeValues.text = string.Join(Environment.NewLine, dict.Values);
        }

        private Sprite GetIcon(string itemName)
        {
            return IconCollectionExample.FirstOrDefault(i => i.name == itemName);
        }
    }
}