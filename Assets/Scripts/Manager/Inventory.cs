using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TrustfallGames.KeepTalkingAndEscape.Manager {


    public class Inventory : MonoBehaviour {

        private int column = 0;
        private int row = 0;
        [SerializeField] private GameObject[] _line1 = new GameObject[5];
        [SerializeField] private GameObject[] _line2 = new GameObject[5];
        [SerializeField] private GameObject[] _line3 = new GameObject[5];
        [SerializeField] private GameObject[] _line4 = new GameObject[5];
        [SerializeField] private Image _emptyItemSlot;
        [SerializeField] private Image _ghostSelectorOutline;
        [SerializeField] private Image _humanSelectorOutline;

        //Array der Inventarslots [zeile, spalte]
        private GameObject[,] _slots = new GameObject[4, 5];

        // Use this for initialization
        void Start() {
            ValidateData();
        }

        private void CreateSlotsArray() {
            for(int j = 0; j < _line1.Length-1; j++) {
                _slots[0, j] = _line1[j];
                _slots[1, j] = _line2[j];
                _slots[2, j] = _line3[j];
                _slots[3, j] = _line4[j];
            }
        }

        private void ValidateData() {
            if(_line1.Length == _line2.Length && _line1.Length == _line3.Length && _line1.Length == _line4.Length) {
                CreateSlotsArray();
            }
            else {
                throw new ArgumentException("Line Arrays must have the same length");
            }
        }

        // Update is called once per frame
        void Update() {
            if(Input.GetButtonDown(ButtonNames.GhostVerticalPad))
        }
    }
}
