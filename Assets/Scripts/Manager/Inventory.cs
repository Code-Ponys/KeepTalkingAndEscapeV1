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


            }
            else {
            }
        }

        // Update is called once per frame
        void Update() {
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
