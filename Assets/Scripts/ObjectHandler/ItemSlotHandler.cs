using System;
using TrustfallGames.KeepTalkingAndEscape.Datatypes;
using TrustfallGames.KeepTalkingAndEscape.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace TrustfallGames.KeepTalkingAndEscape.Listener {
    public class ItemSlotHandler : MonoBehaviour {
        private Item _item;
        private Image _image;
        private SpriteRenderer _spriteRenderer;
        private string _currentitemId;
        private Inventory _inventory;
        private CharacterType _characterType;

        private void Start() {
            _image = gameObject.GetComponent<Image>();
        }

        private void Update() {
            if(_inventory == null) _inventory = Inventory.GetInstance(_characterType);

            if(_item == null && _currentitemId == "") return;
            if(_item == null && _currentitemId != "") {
                _image.sprite = _inventory.EmptyItemSlot;
                _currentitemId = "";
                return;
            }

            if(string.Equals(_item.ItemId, _currentitemId, StringComparison.CurrentCultureIgnoreCase)) return;
            _image.sprite = Resources.Load<Sprite>(_item.SpritePath);
            _currentitemId = _item.ItemId;
        }

        public Datatypes.Item Item {
            get {return _item;}
            set {_item = value;}
        }

        public CharacterType CharacterType {
            get {return _characterType;}
            set {_characterType = value;}
        }

    }
}
