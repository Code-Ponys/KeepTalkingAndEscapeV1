using System;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

namespace TrustfallGames.KeepTalkingAndEscape.Manager {


    public class Inventory : MonoBehaviour {

        //Array der Inventarslots [zeile, spalte]
        private readonly GameObject[,] _slots = new GameObject[4, 5];
        private readonly GameObject[,] _selectorHuman = new GameObject[4, 5];
        private readonly GameObject[,] _selectorGhost = new GameObject[4, 5];

        [SerializeField] private float _axisDelay = 0.5f;
        [SerializeField] private CharacterType _characterType;

        private float _currentAxisDelay;
        [SerializeField] private Sprite _emptyItemSlot;
        [SerializeField] private Sprite _ghostSelectorOutline;
        [SerializeField] private Sprite _humanSelectorOutline;

        private int _x = 0;
        private int _y = 0;

        // Use this for initialization
        private void Start() {
            SearchInventoryObjects();
        }

        private void SearchInventoryObjects() {
            switch(_characterType) {
                case CharacterType.Ghost:
                    for(var i = 0; i < 4; i++) {
                        for(var j = 0; j < 5; j++) {
                            _slots[i, j] = GameObject.Find(i + "," + j + "G");
                            _selectorHuman[i, j] = GameObject.Find(i + "," + j + "GSH");
                            _selectorGhost[i, j] = GameObject.Find(i + "," + j + "GSG");
                        }
                    }

                    Debug.Log("_slots: " + _slots.Length + " | selector Human: " + _selectorHuman.Length + " | selector Ghost: " + _selectorGhost.Length);
                    break;
                case CharacterType.Human:
                    for(var i = 0; i < 4; i++) {
                        for(var j = 0; j < 5; j++) {
                            _slots[i, j] = GameObject.Find(i + "," + j + "H");
                            _selectorHuman[i, j] = GameObject.Find(i + "," + j + "HSH");
                            _selectorGhost[i, j] = GameObject.Find(i + "," + j + "HSG");
                        }
                    }

                    Debug.Log("_slots: " + _slots.Length + " | selector Human: " + _selectorHuman.Length + " | selector Ghost: " + _selectorGhost.Length);
                    break;
                default:
                    throw new Exception("Character type not set.");
            }
        }


        // Update is called once per frame
        private void Update() {
            InventoryInput();
            UpdateSelection();
        private void InventoryVisible() {
            switch(_characterType) {
                case CharacterType.Ghost:
                    if(Input.GetButtonDown(ButtonNames.GhostInventory))
                        _inventoryActive = !_inventoryActive;
                    break;
                case CharacterType.Human:
                    if(Input.GetButtonDown(ButtonNames.HumanInventory))
                        _inventoryActive = !_inventoryActive;
                    break;
                default:
                    throw new Exception("CharacterType must be ghost or human");
            }

            _inventoryVisibleObject.SetActive(_inventoryActive);
        }

        private void UpdateSelection() {
            foreach(var obj in _selectorGhost) {
                obj.GetComponent<Image>().sprite = _transparentSprite;
            }

            foreach(var obj in _selectorHuman) {
                obj.GetComponent<Image>().sprite = _transparentSprite;
            }

            switch(_characterType) {
                case CharacterType.Ghost:
                    _selectorGhost[_y, _x].GetComponent<Image>().sprite = _ghostSelectorOutline;
                    break;
                case CharacterType.Human:
                    _selectorHuman[_y, _x].GetComponent<Image>().sprite = _humanSelectorOutline;
                    break;
            }

            if(_secondInventory.InventoryActive) {
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
        }

        /// <summary>
        /// The input function for the ui manager.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        private void InventoryInput() {
            //Button input for human
            switch(_characterType) {
                case CharacterType.Human:
                    Debug.Log("Input Humyn Y: " + Input.GetAxis(ButtonNames.MoveHumanY));
                    Debug.Log("Input Human X: " + Input.GetAxis(ButtonNames.MoveHumanX));
                    //Change current choosed Item
                    if(_currentAxisDelay < 0) {
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

                    break;
                case CharacterType.Ghost:
                    //Change current choosed Item
                    if(_currentAxisDelay <= 0) {
                        Debug.Log("Input Ghost Y: " + Input.GetAxis(ButtonNames.MoveGhostY));
                        Debug.Log("Input Ghost X: " + Input.GetAxis(ButtonNames.MoveGhostX));
                        if(Input.GetAxis(ButtonNames.MoveGhostX) < 0) {
                            //Left
                            Debug.Log("Pressed Left");
                            if(_x == 0) return;
                            _x--;
                            _currentAxisDelay = _axisDelay;
                        }

                        if(Input.GetAxis(ButtonNames.MoveGhostX) > 0) {
                            //Right
                            Debug.Log("Pressed right");
                            if(_x == 4) return;
                            _x++;
                            _currentAxisDelay = _axisDelay;
                        }

                        if(Input.GetAxis(ButtonNames.MoveGhostY) < 0) {
                            //Down
                            Debug.Log("Pressed Down");
                            if(_y == 3) return;
                            _y++;
                            _currentAxisDelay = _axisDelay;
                        }

                        if(Input.GetAxis(ButtonNames.MoveGhostY) > 0) {
                            //Up
                            Debug.Log("Pressed Up");
                            if(_y == 0) return;
                            _y--;
                            _currentAxisDelay = _axisDelay;
                        }
                    }
                    else {
                        _currentAxisDelay -= Time.deltaTime;
                    }

                    break;
            }
        }
    }
}
