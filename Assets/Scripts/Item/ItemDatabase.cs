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
                var obj = new Item {
                    ItemId = "testitem" + i,
                    Name = "testitem " + i,
                    SpritePath = "assets/2D_Art/itempics/sendnudes.jpg",
                    GhostDescription = "testitem" + i + " beschreibung geist",
                    HumanDescription = "testitem" + i + " beschreibung human",
                    Combineable = true,
                    CombineWith = CombineWith.Item,
                    CombineWithItem = "testitem " + (i + 1),
                    NextItem = "testitem " + (i + 2)
                };
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
