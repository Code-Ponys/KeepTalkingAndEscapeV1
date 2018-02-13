namespace TrustfallGames.KeepTalkingAndEscape.Datatypes {
    public class Item {

        //Item Name
        private string _name;

        //Path to sprite ./Resources/...
        private string _spritePath;

        //Item ID. Must be unique and entered in Defines
        private string _itemId;

        //Is the object Combineable with another Item.
        private bool _combineable;

        //Ist the item with an item combineable or must it be cleaned
        private CombineWith _combineWith;

        //With which item is the Item combineable. If not write 'none'
        private string _combineWithItem;

        //which item should you get, if you combine the items.
        private string _nextItem;

        //Description of the item for ghost.
        private string _ghostDescription;

        //Description of the Item for human.
        private string _humanDescription;

        public string ItemId {
            get {return _itemId;}
            set {_itemId = value;}
        }

        public string Name {
            get {return _name;}
            set {_name = value;}
        }

        public string SpritePath {
            get {return _spritePath;}
            set {_spritePath = value;}
        }

        public string GhostDescription {
            get {return _ghostDescription;}
            set {_ghostDescription = value;}
        }

        public string HumanDescription {
            get {return _humanDescription;}
            set {_humanDescription = value;}
        }

        public bool Combineable {
            get {return _combineable;}
            set {_combineable = value;}
        }

        public CombineWith CombineWith {
            get {return _combineWith;}
            set {_combineWith = value;}
        }

        public string CombineWithItem {
            get {return _combineWithItem;}
            set {_combineWithItem = value;}
        }

        public string NextItem {
            get {return _nextItem;}
            set {_nextItem = value;}
        }
    }
}
