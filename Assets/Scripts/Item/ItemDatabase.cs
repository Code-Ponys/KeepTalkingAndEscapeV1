using System;
using System.Collections.Generic;
using TrustfallGames.KeepTalkingAndEscape.DataController;
using TrustfallGames.KeepTalkingAndEscape.Listener;
using TrustfallGames.KeepTalkingAndEscape.Manager;
using UnityEngine;

namespace TrustfallGames.KeepTalkingAndEscape.Datatypes {
    [Serializable]
    public class ItemDatabase {

        [SerializeField]
        private List<Item> _itemDatabase;

        private int _dataBaseVersion;
        private string _language;

        public void GenerateTestdatabase() {
            DataBaseVersion = 1;
            Language = "de_DE";
            
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

        public int DataBaseVersion {
            get {return _dataBaseVersion;}
            set {_dataBaseVersion = value;}
        }

        public string Language {
            get {return _language;}
            set {_language = value;}
        }

        public static ItemDatabase GetInstance() {
            return GameObject.Find("ItemHandler").GetComponent<ItemManager>().ItemDatabase;
        }
        
        public List<Item> ItemDatabaseList {
            get {return _itemDatabase;}
            set {_itemDatabase = value;}
        }

    }
}
