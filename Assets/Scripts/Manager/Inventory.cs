using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        void Start() {
            ValidateData();
        }

        private void SearchInventoryObjects() {
            if(_characterType == CharacterType.Ghost) {
                for(int i = 0; i < 4; i++) {
                    for(int j = 0; j < 5; j++) {
                        _slots[i, j] = GameObject.Find(i + "," + j + "G");
                        _selectorHuman[i, j] = GameObject.Find(i + "," + j + "GSH");
                        _selectorGhost[i, j] = GameObject.Find(i + "," + j + "GSG");
                    }
                }

                Debug.Log("_slots: " + _slots.Length + " | selector Human: " + _selectorHuman.Length + " | selector Ghost: " + _selectorGhost.Length);
            }
            else if(_characterType == CharacterType.Human) {
                for(int i = 0; i < 4; i++) {
                    for(int j = 0; j < 5; j++) {
                        _slots[i, j] = GameObject.Find(i + "," + j + "H");
                        _selectorHuman[i, j] = GameObject.Find(i + "," + j + "HSH");
                        _selectorGhost[i, j] = GameObject.Find(i + "," + j + "HSG");
                    }
                }
            }
            else {
                throw new ArgumentException("Character type not set.");
            }
        }

        // Update is called once per frame
        void Update() {
        private void Update() {
            InventoryInput();
            UpdateSelection();
        }

        private void UpdateSelection() {
            if(_characterType == CharacterType.Ghost) {
                _selectorGhost[_y, _x].GetComponent<Image>().sprite = _ghostSelectorOutline;
                Debug.Log("Change");
            }
            else if(_characterType == CharacterType.Human) {
                _selectorHuman[_y, _x].GetComponent<Image>().sprite = _humanSelectorOutline;
            }
        }

            if(_characterType == CharacterType.Human) {
                if(Input.GetAxis(ButtonNames.GhostVerticalPad) < 0) {
                }
            }

            else if(_characterType == CharacterType.Ghost) {
            }
            else {
                throw new ArgumentException("Character type not set.");
            }
        }
    }
}
