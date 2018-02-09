namespace TrustfallGames.KeepTalkingAndEscape.Datatypes {
    public class Item {

        //Item Name
        private string _name;

        //Path to sprite ./Resources/...
        private string _spritePath;

        //Item ID. Must be unique and entered in Defines
        private ItemId _itemId;

        //Is the object Combineable with another Item.
        private bool _combineable;

        //Ist the item with an item combineable or must it be cleaned
        private CombineWith _combineWith;

        //With which item is the Item combineable. If not write 'none'
        private ItemId _combineWithItem;

        //which item should you get, if you combine the items
        private ItemId _nextItem;

        public string Name {
            get {return _name;}
            set {_name = value;}
        }

        public string SpritePath {
            get {return _spritePath;}
            set {_spritePath = value;}
        }

        public ItemId ItemId {
            get {return _itemId;}
            set {_itemId = value;}
        }

        public bool Combineable {
            get {return _combineable;}
            set {_combineable = value;}
        }

        public CombineWith CombineWith {
            get {return _combineWith;}
            set {_combineWith = value;}
        }

        public ItemId CombineWithItem {
            get {return _combineWithItem;}
            set {_combineWithItem = value;}
        }

        public ItemId NextItem {
            get {return _nextItem;}
            set {_nextItem = value;}
        }
    }
}
