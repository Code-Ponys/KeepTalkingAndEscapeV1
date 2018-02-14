using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrustfallGames.KeepTalkingAndEscape.Manager {
    public class MainMenu : MonoBehaviour {

        [SerializeField] private float _axisDelay = 0.5f;
        [SerializeField] private CharacterType _characterType;
        private float _currentAxisDelay;
        private int _x = 0;
        private int _y = 0;

        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {
        }

        private void InventoryInput() {
            //Button input for human
            switch(_characterType) {
                case CharacterType.Human:
                    //Change current button
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
                    }
                    else {
                        _currentAxisDelay -= Time.deltaTime;
                    }

                    break;
                case CharacterType.Ghost:
                    //Change current button
                    if(_currentAxisDelay <= 0) {
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
                    }
                    else {
                        _currentAxisDelay -= Time.deltaTime;
                    }

                    break;
            }
        }
    }
}
