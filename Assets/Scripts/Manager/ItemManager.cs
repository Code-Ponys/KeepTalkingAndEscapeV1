using System;
using System.Collections.Generic;
using TrustfallGames.KeepTalkingAndEscape.DataController;
using TrustfallGames.KeepTalkingAndEscape.Datatypes;
using TrustfallGames.KeepTalkingAndEscape.Listener;
using TrustfallGames.KeepTalkingAndEscape.Manager;
using UnityEngine;

namespace TrustfallGames.KeepTalkingAndEscape.Manager {
    public class ItemManager : MonoBehaviour {
        //All Items which can exist.
        private ObjectInteractionListener _objectInteractionListener;
        private ItemDatabase _itemDatabase;
        private GameManager _gameManager;
        private SoundManager _soundManager;
        [SerializeField] private int _itemsInDatabase;
        private Queue<ObjectInteractionListener> _itemcheck = new Queue<ObjectInteractionListener>();

        private List<Item> _itemList;

        //The current Items in the Inventory

        private List<Item> _inventory = new List<Item>();
        private UIManager _uiManager;

        public static ItemManager GetItemManager() {
            return GameObject.Find("System").GetComponent<ItemManager>();
        }

        private void Start() {
            _soundManager = SoundManager.GetSoundManager();
            _itemDatabase = new ItemDatabase();
            _itemDatabase = ItemDatabaseHandler.LoadDataBase();
            _itemList = _itemDatabase.ItemDatabaseList;
            _gameManager = GameManager.GetGameManager();
            _uiManager = UIManager.GetUiManager();
        }

        private void Update() {
            _itemsInDatabase = _itemList.Count;
            if(_itemcheck.Count != 0) {
                var obj = _itemcheck.Dequeue();
                if(!IsItemInDatabase(obj.ItemName)) {
                    throw new Exception("The item " + obj.ItemName + " is not in Database. It's assigned to Object Listener of " + obj.gameObject.name + ". Please Check the ID in Inspector and Database");
                }
            }
        }

        /// <summary>
        /// If items are combineable, it combines them, adds the new item to the inventory, and remove the combined items.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns>Returns true, if the combination was valid.</returns>
        public bool ItemsCombineable(Item item1, Item item2) {
            if(!item1.Combineable || !item2.Combineable) return false;
            if(item1.CombineWith != CombineWith.Item || item2.CombineWith != CombineWith.Item) {
                _soundManager.Source.clip = _soundManager.FailComboSound;
                _soundManager.Source.Play();
                return false;
            }
            if(item1.CombineWithItem != item2.ItemId || item2.CombineWithItem != item1.ItemId) {
                _soundManager.Source.clip = _soundManager.FailComboSound;
                _soundManager.Source.Play();
                return false;
            }
            _soundManager.Source.clip = _soundManager.SuccessComboSound;
            _soundManager.Source.Play();
            AddItemToInv(item1.NextItem);
            RemoveItemFromInventory(item1.ItemId);
            RemoveItemFromInventory(item2.ItemId);
            Debug.Log("Items combined");
            return true;
        }

        /// <summary>
        /// Search in the item database for the item and adds it to the inventory.
        /// </summary>
        /// <param name="Item you want to add"></param>
        public void AddItemToInv(string itemId) {
            foreach(var obj in _itemList) {
                if(!string.Equals(obj.ItemId, itemId, StringComparison.CurrentCultureIgnoreCase)) continue;
                _inventory.Add(obj);
                _uiManager.InventoryHuman.ItemInHand = "";
                _uiManager.DisplayLastPickedUpItem(obj);
                _uiManager.InventoryGhost.RearrangeItems();
                _uiManager.InventoryHuman.RearrangeItems();
                return;
            }


            throw new ArgumentException("Item is not in Database. Please Check you database file.");
        }

        /// <summary>
        /// Removes the Item from the inventory.
        /// </summary>
        /// <param name="itemId"></param>
        private void RemoveItemFromInventory(string itemId) {
            for(var i = 0; i < _inventory.Count; i++) {
                var obj = _inventory[i];
                if(string.Equals(obj.ItemId, itemId, StringComparison.CurrentCultureIgnoreCase)) {
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
        public Item GetItemFromDatabase(string itemId) {
            foreach(var obj in _itemList)
                if(string.Equals(obj.ItemId, itemId, StringComparison.CurrentCultureIgnoreCase))
                    return obj;

            throw new ArgumentException("Item is not in Database. Please check the database file.");
        }

        private bool IsItemInDatabase(string itemId) {
            if(itemId == "") return true;
            foreach(var obj in _itemList) {
                if(string.Equals(obj.ItemId, itemId, StringComparison.CurrentCultureIgnoreCase)) {
                    return true;
                }
            }

            return false;
        }

        public void CheckItem(ObjectInteractionListener obj) {
            _itemcheck.Enqueue(obj);
        }


        public List<Item> Inventory {
            get {return _inventory;}
            set {_inventory = value;}
        }

        public List<Item> ItemsDatabase {
            get {return _itemList;}
            set {_itemList = value;}
        }

        public ItemDatabase ItemDatabase {
            get {return _itemDatabase;}
            set {_itemDatabase = value;}
        }

        public void RemoveItemFromHandAndInventory() {
            RemoveItemFromInventory(_uiManager.InventoryHuman.ItemInHand);
            _uiManager.InventoryHuman.ItemInHand = "";
        }
    }
}
