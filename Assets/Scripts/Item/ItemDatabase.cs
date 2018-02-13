using System.Collections.Generic;
using TrustfallGames.KeepTalkingAndEscape.DataController;
using UnityEngine;

namespace TrustfallGames.KeepTalkingAndEscape.Datatypes {
    public class ItemDatabase : MonoBehaviour {

        private List<Item> _itemDatabase;

        private ItemDatabase() {
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
