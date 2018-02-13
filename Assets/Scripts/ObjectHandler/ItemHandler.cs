using System;
using System.Collections.Generic;
using TrustfallGames.KeepTalkingAndEscape.Datatypes;
using UnityEditor;
using UnityEngine;

namespace TrustfallGames.KeepTalkingAndEscape.Listener {
    public class ItemHandler : MonoBehaviour {
        //All Items which can exist.
        private List<Item> _itemsDatabase = new List<Item>();
        //The current Items in the Inventory
        private List<Item> _inventory = new List<Item>();

        public List<Item> Inventory {
            get {return _inventory;}
            set {_inventory = value;}
        }

        public List<Item> ItemsDatabase {
            get {return _itemsDatabase;}
            set {_itemsDatabase = value;}
        }

        /// <summary>
        /// If items are combineable, it combines them, adds the new item to the inventory, and remove the combined items.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns>Returns true, if the combination was valid.</returns>
        public bool ItemsCombineable(Item item1, Item item2) {
            if(!item1.Combineable || !item2.Combineable) return false;
            if(item1.CombineWith != CombineWith.Item || item2.CombineWith != CombineWith.Item) return false;
            if(item1.CombineWithItem != item2.ItemId || item2.CombineWithItem != item1.ItemId) return false;
            AddItemToInv(item1.NextItem);
            RemoveItemFromInventory(item1.ItemId);
            RemoveItemFromInventory(item2.ItemId);
            return true;

            return false;
        }

        /// <summary>
        /// Search in the item database for the item and adds it to the inventory.
        /// </summary>
        /// <param name="Item you want to add"></param>
        public void AddItemToInv(string itemId) {
            foreach(var obj in _itemDatabase.ItemDatabaseList) {
                if(!string.Equals(obj.ItemId, itemId, StringComparison.CurrentCultureIgnoreCase)) throw new ArgumentException("Item is not in Databse. Please Check you database file.");
                _inventory.Add(obj);
                return;

            }
        }

        /// <summary>
        /// Removes the Item from the inventory.
        /// </summary>
        /// <param name="itemId"></param>
        private void RemoveItemFromInventory(string itemId) {
            for(int i = 0; i < _inventory.Count; i++) {
                var obj = _inventory[i];
                if(String.Equals(obj.ItemId, itemId, StringComparison.CurrentCultureIgnoreCase)) {
                    _inventory.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Returns the item object with the itemID
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private Item GetItemFromDatabase(string itemId) {
            foreach(var obj in _itemDatabase.ItemDatabaseList) {
                if(String.Equals(obj.ItemId, itemId, StringComparison.CurrentCultureIgnoreCase)) {
                    return obj;
                }
            }
            throw new ArgumentException("Item is not in Database. Please check the database file.");
        } 
        public List<Item> Inventory {
            get {return _inventory;}
            set {_inventory = value;}
        }

        public List<Item> ItemsDatabase {
            get {return _itemDatabase.ItemDatabaseList;}
            set {_itemDatabase.ItemDatabaseList = value;}
        }
    }
}
