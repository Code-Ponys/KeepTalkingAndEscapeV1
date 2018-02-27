using System;
using TrustfallGames.KeepTalkingAndEscape.Listener;
using UnityEngine;
using UnityEngine.UI;

namespace TrustfallGames.KeepTalkingAndEscape.Manager {


    public class Inventory : MonoBehaviour {

        //Array der Inventarslots [zeile, spalte]
        private readonly ItemSlotHandler[,] _slots = new ItemSlotHandler[4, 5];
        private readonly Image[,] _selectorHuman = new Image[4, 5];
        private readonly Image[,] _selectorGhost = new Image[4, 5];

        [SerializeField] private float _axisDelay = 0.5f;
        [SerializeField] private CharacterType _characterType;
        [SerializeField] private Inventory _secondInventory;
        [SerializeField] private GameObject _inventoryVisibleObject;
        [SerializeField] private Text _itemName;
        [SerializeField] private Text _itemText;
        [SerializeField] private Text _itemCombineText;


        private ItemManager _itemManager;

        private float _currentAxisDelay;
        [SerializeField] private Sprite _emptyItemSlot;
        [SerializeField] private Sprite _ghostSelectorOutline;
        [SerializeField] private Sprite _humanSelectorOutline;
        [SerializeField] private Sprite _transparentSprite;
        private bool _inventoryActive = true;
        private bool _lastInventoryState;

        [SerializeField] private string _itemInHand;

        private int _x = 0;
        private int _y = 0;

        private GameObject[] combine = new GameObject[2];
        private GameManager _gameManager;

        // Use this for initialization

        private void Start() {
            SearchInventoryObjects();

            if(_secondInventory.CharacterType == _characterType) throw new Exception("Inventory must have different character types");

            _itemManager = ItemManager.GetItemManager();
            _gameManager = GameManager.GetGameManager();

            var canvas = gameObject.GetComponent<Canvas>();

            switch(_characterType) {
                case CharacterType.Ghost:
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvas.worldCamera = _gameManager.GhostCamera;
                    break;
                case CharacterType.Human:
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvas.worldCamera = _gameManager.HumanCamera;
                    break;
                case CharacterType.Unassigned:
                    throw new Exception("Inventory is missing a character type");
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ToggleInventoryVisibility();
        }

        // Update is called once per frame
        private void Update() {
            InventoryVisible();
            if(!_inventoryActive) return;
            UpdateSelection();
            InventoryInput();
            UpdateUI();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void UpdateUI() {
            if(_slots[_y, _x].Item != null) {
                var item = _slots[_y, _x].Item;
                _itemName.text = item.Name;
                if(_characterType == CharacterType.Ghost)
                    _itemText.text = item.GhostDescription;
                else
                    _itemText.text = item.HumanDescription;
            }
            else {
                _itemName.text = "";
                _itemText.text = "";
            }
        }

        /// <summary>
        /// Toggles Inventory visibility. Rearranges the items, if the other inventory is closed
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void InventoryVisible() {
            switch(_characterType) {
                case CharacterType.Ghost:
                    if(Input.GetButtonDown(ButtonNames.GhostInventory)) {
                        ToggleInventoryVisibility();
                    }

                    break;
                case CharacterType.Human:
                    if(Input.GetButtonDown(ButtonNames.HumanInventory)) {
                        ToggleInventoryVisibility();
                    }

                    break;
                default:
                    throw new Exception("CharacterType must be ghost or human");
            }
        }

        private void ToggleInventoryVisibility() {
            _inventoryActive = !_inventoryActive;
            if(_inventoryActive && !_secondInventory._inventoryActive) RearrangeItems();
            _itemCombineText.text = "";

            _x = 0;
            _y = 0;
            combine[0].GetComponent<ItemCombineSlotHandler>().Item = null;
            combine[1].GetComponent<ItemCombineSlotHandler>().Item = null;

            _inventoryVisibleObject.SetActive(_inventoryActive);
        }

        /// <summary>
        /// Search all Inventory Objects for init.
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void SearchInventoryObjects() {
            switch(_characterType) {
                case CharacterType.Ghost:
                    for(var i = 0; i < 4; i++)
                    for(var j = 0; j < 5; j++) {
                        _slots[i, j] = GameObject.Find("GhostInventory/VisibilityObject/Slots/InventoryItems/" + i + "," + j).AddComponent<ItemSlotHandler>();
                        _selectorHuman[i, j] = GameObject.Find("GhostInventory/VisibilityObject/Slots/InventorySelectorHuman/" + i + "," + j).GetComponent<Image>();
                        _selectorGhost[i, j] = GameObject.Find("GhostInventory/VisibilityObject/Slots/InventorySelectorGhost/" + i + "," + j).GetComponent<Image>();
                        _slots[i, j].CharacterType = CharacterType.Ghost;
                    }

                    combine[0] = GameObject.Find("GhostInventory/VisibilityObject/Slots/Combine/First");
                    combine[1] = GameObject.Find("GhostInventory/VisibilityObject/Slots/Combine/Second");
                    combine[0].AddComponent<ItemCombineSlotHandler>().CharacterType = CharacterType.Human;
                    combine[1].AddComponent<ItemCombineSlotHandler>().CharacterType = CharacterType.Human;

                    Debug.Log("_slots: " + _slots.Length + " | selector Human: " + _selectorHuman.Length + " | selector Ghost: " + _selectorGhost.Length);
                    break;
                case CharacterType.Human:
                    for(var i = 0; i < 4; i++)
                    for(var j = 0; j < 5; j++) {
                        _slots[i, j] = GameObject.Find("HumanInventory/VisibilityObject/Slots/InventoryItems/" + i + "," + j).AddComponent<ItemSlotHandler>();
                        _selectorHuman[i, j] = GameObject.Find("HumanInventory/VisibilityObject/Slots/InventorySelectorHuman/" + i + "," + j).GetComponent<Image>();
                        _selectorGhost[i, j] = GameObject.Find("HumanInventory/VisibilityObject/Slots/InventorySelectorGhost/" + i + "," + j).GetComponent<Image>();
                        _slots[i, j].CharacterType = CharacterType.Human;
                    }

                    combine[0] = GameObject.Find("HumanInventory/VisibilityObject/Slots/Combine/First");
                    combine[1] = GameObject.Find("HumanInventory/VisibilityObject/Slots/Combine/Second");
                    combine[0].AddComponent<ItemCombineSlotHandler>().CharacterType = CharacterType.Human;
                    combine[1].AddComponent<ItemCombineSlotHandler>().CharacterType = CharacterType.Human;

                    break;
                default:
                    throw new Exception("Character type not set.");
            }
        }


        /// <summary>
        /// Updates the selection for both characters
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        private void UpdateSelection() {
            foreach(var obj in _selectorGhost) obj.GetComponent<Image>().sprite = _transparentSprite;

            foreach(var obj in _selectorHuman) obj.GetComponent<Image>().sprite = _transparentSprite;

            switch(_characterType) {
                case CharacterType.Ghost:
                    _selectorGhost[_y, _x].GetComponent<Image>().sprite = _ghostSelectorOutline;
                    break;
                case CharacterType.Human:
                    _selectorHuman[_y, _x].GetComponent<Image>().sprite = _humanSelectorOutline;
                    break;
            }

            if(_secondInventory.InventoryActive)
                switch(_secondInventory.CharacterType) {
                    case CharacterType.Ghost:
                        _selectorGhost[_secondInventory.Y, _secondInventory.X].GetComponent<Image>().sprite = _ghostSelectorOutline;
                        break;
                    case CharacterType.Human:
                        _selectorHuman[_secondInventory.Y, _secondInventory.X].GetComponent<Image>().sprite = _humanSelectorOutline;
                        break;
                    default:
                        throw new ArgumentException("Your second Inventory does not have a valid character type.");
                }
        }

        /// <summary>
        /// The input function for the ui manager.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        private void InventoryInput() {
            //Button input for human
            switch(_characterType) {
                case CharacterType.Human:
                    InputHuman();

                    break;
                case CharacterType.Ghost:
                    InputGhost();

                    break;
            }
        }

        private void InputGhost() {
            if(_currentAxisDelay <= 0) {
                //Change current choosed Item
                if(Input.GetAxis(ButtonNames.MoveGhostX) < 0) {
                    //Left
                    if(_x == 0) return;
                    _x--;
                    _currentAxisDelay = _axisDelay;
                }

                if(Input.GetAxis(ButtonNames.MoveGhostX) > 0) {
                    //Right
                    if(_x == 4) return;
                    _x++;
                    _currentAxisDelay = _axisDelay;
                }

                if(Input.GetAxis(ButtonNames.MoveGhostY) < 0) {
                    //Down
                    if(_y == 3) return;
                    _y++;
                    _currentAxisDelay = _axisDelay;
                }

                if(Input.GetAxis(ButtonNames.MoveGhostY) > 0) {
                    //Up
                    if(_y == 0) return;
                    _y--;
                    _currentAxisDelay = _axisDelay;
                }
            }
            else {
                _currentAxisDelay -= Time.deltaTime;
            }
        }

        private void InputHuman() {
            if(_currentAxisDelay < 0) {
                //Change current choosed Item
                if(Input.GetAxis(ButtonNames.MoveHumanX) < 0) {
                    //Left
                    if(_x == 0) return;
                    _x--;
                    _currentAxisDelay = _axisDelay;
                }
                else if(Input.GetAxis(ButtonNames.MoveHumanX) > 0) {
                    //Right
                    if(_x == 4) return;
                    _x++;
                    _currentAxisDelay = _axisDelay;
                }

                if(Input.GetAxis(ButtonNames.MoveHumanY) < 0) {
                    //Down
                    if(_y == 3) return;
                    _y++;
                    _currentAxisDelay = _axisDelay;
                }
                else if(Input.GetAxis(ButtonNames.MoveHumanY) > 0) {
                    //Up
                    if(_y == 0) return;
                    _y--;
                    _currentAxisDelay = _axisDelay;
                }
            }
            else {
                _currentAxisDelay -= Time.deltaTime;
            }

            if(Input.GetButtonDown(ButtonNames.HumanInspect))
                if(_slots[_y, _x].GetComponent<ItemSlotHandler>().Item != null) {
                    if(combine[0].GetComponent<ItemCombineSlotHandler>().Item == null) {
                        combine[0].GetComponent<ItemCombineSlotHandler>().Item = _slots[_y, _x].GetComponent<ItemSlotHandler>().Item;
                        _secondInventory.combine[0].GetComponent<ItemCombineSlotHandler>().Item = _slots[_y, _x].GetComponent<ItemSlotHandler>().Item;
                    }
                    else {
                        combine[1].GetComponent<ItemCombineSlotHandler>().Item = _slots[_y, _x].GetComponent<ItemSlotHandler>().Item;
                        _secondInventory.combine[1].GetComponent<ItemCombineSlotHandler>().Item = _slots[_y, _x].GetComponent<ItemSlotHandler>().Item;
                    }

                    if(combine[0].GetComponent<ItemCombineSlotHandler>().Item != null && combine[1].GetComponent<ItemCombineSlotHandler>().Item != null) {
                        Debug.Log("Trying to combine");
                        if(_itemManager.ItemsCombineable(combine[0].GetComponent<ItemCombineSlotHandler>().Item, combine[1].GetComponent<ItemCombineSlotHandler>().Item)) {
                            var item = _itemManager.GetItemFromDatabase(combine[0].GetComponent<ItemCombineSlotHandler>().Item.NextItem);
                            _itemCombineText.text = "Kombinieren erfolgreich. " + item.Name + " erhalten";
                            _secondInventory._itemCombineText.text = "Kombinieren erfolgreich. " + item.Name + " erhalten";
                            RearrangeItems();
                        }
                        else {
                            _itemCombineText.text = "Diese Gegenstände können nicht kombiniert werden.";
                        }

                        combine[0].GetComponent<ItemCombineSlotHandler>().Item = null;
                        combine[1].GetComponent<ItemCombineSlotHandler>().Item = null;
                        if(_characterType == CharacterType.Human) {
                            _secondInventory.combine[0].GetComponent<ItemCombineSlotHandler>().Item = null;
                            _secondInventory.combine[1].GetComponent<ItemCombineSlotHandler>().Item = null;
                        }
                    }
                }

            if(Input.GetButtonDown(ButtonNames.HumanJoystickButtonX)) {
                _itemInHand = _slots[_y, _x].Item.ItemId;
                ToggleInventoryVisibility();
            }
        }

        /// <summary>
        /// Rearrange all items in both inventories
        /// </summary>
        public void RearrangeItems() {
            var list = _itemManager.Inventory;
            var listcount = 0;
            foreach(var obj in _slots) {
                obj.GetComponent<ItemSlotHandler>().Item = listcount > list.Count - 1 ? null : list[listcount];

                listcount++;
            }

            listcount = 0;
            foreach(var obj in _secondInventory._slots) {
                obj.GetComponent<ItemSlotHandler>().Item = listcount > list.Count - 1 ? null : list[listcount];

                listcount++;
            }
        }

        public static Inventory GetInstance() {
            return GameObject.Find("GhostInventory").GetComponent<Inventory>();
        }

        private CharacterType CharacterType {
            get {return _characterType;}
        }

        public Sprite EmptyItemSlot {
            get {return _emptyItemSlot;}
        }

        private int X {
            get {return _x;}
        }

        private int Y {
            get {return _y;}
        }

        public bool InventoryActive {
            get {return _inventoryActive;}
            set {_inventoryActive = value;}
        }

        public string ItemInHand {
            get {return _itemInHand;}
            set {_itemInHand = value;}
        }

        public static Inventory GetInstance(CharacterType characterType) {
            if(characterType == CharacterType.Ghost) return GameObject.Find("GhostInventory").GetComponent<Inventory>();

            return GameObject.Find("HumanInventory").GetComponent<Inventory>();
        }
    }
}
