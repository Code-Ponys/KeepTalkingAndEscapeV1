using System.Collections.Generic;
using TrustfallGames.KeepTalkingAndEscape.DataController;
using UnityEngine;

namespace TrustfallGames.KeepTalkingAndEscape.Datatypes {
    public class ItemDatabase : MonoBehaviour {

        private List<Item> _itemDatabase;

        private ItemDatabase() {
        }

        private void Awake() {
            _itemDatabase = new List<Item>();
            for(var i = 1; i < 11; i++) {
                var obj = new Item();
                obj.ItemId = "testitem" + i;
                obj.Name = "testitem " + i;
                obj.SpritePath = "assets/2D_Art/itempics/sendnudes.jpg";
                obj.GhostDescription = "testitem" + i + " beschreibung geist";
                obj.HumanDescription = "testitem" + i + " beschreibung human";
                obj.Combineable = true;
                obj.CombineWith = CombineWith.Item;
                obj.CombineWithItem = "testitem " + i + 1;
                obj.NextItem = "testitem " + i + 2;
                _itemDatabase.Add(obj);
            }
            ItemDatabaseHandler.WriteDatabase();
        }


        public static ItemDatabase GetInstance() {
            return GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
        }

        public List<Item> ItemDatabaseList {
            get {return _itemDatabase;}
            set {_itemDatabase = value;}
        }

    }
}
