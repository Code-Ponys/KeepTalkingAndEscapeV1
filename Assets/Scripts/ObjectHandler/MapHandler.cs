using System;
using System.Collections.Generic;
using TrustfallGames.KeepTalkingAndEscape.Datatypes;
using TrustfallGames.KeepTalkingAndEscape.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace TrustfallGames.KeepTalkingAndEscape.Listener {
    public class MapHandler : MonoBehaviour {
        private List<MapButton> _buttons = new List<MapButton>();
        private float _currentAxisDelay;
        [SerializeField] private string _password;
        [SerializeField] private GameObject _visibilityObject;
        [SerializeField] private Sprite _highlight;
        [SerializeField] private Sprite _transpa;
        [SerializeField] private float _axisDelay;
        private string _passwordText;

        private MapButton _currentActiveButton;
        private bool _codeSolved;

        private GameManager _gameManager;
        private bool _humanMapActive;

        private bool _closedFirstTime;
        private float _wait = 0.5f;
        [SerializeField] private float _closeTimeAfterCodeSolved;


        public void RegisterButton(MapButton button) {
            _buttons.Add(button);
            _gameManager = GameManager.GetGameManager();
        }


        private void Update() {
            if(_visibilityObject.activeSelf) {
                if(_wait < 0) {
                    InputMap();
                }
                else {
                    _wait = _wait - Time.deltaTime;
                }
            }
            AutoClose();
        }

        private void LateUpdate() {
            if(!_closedFirstTime) {
                _buttons[0].ToggleActive();
                _currentActiveButton = _buttons[0];
                CloseMap();
                _closedFirstTime = true;
            }
        }

        public void InputMap() {
            if(_codeSolved) return;
            if(_currentAxisDelay < 0) {
                if(Input.GetAxis(ButtonNames.MoveHumanX) < 0) {
                    //Left
                    _currentAxisDelay = _axisDelay;
                    ChangeActiveButton(Direction.Left);
                }
                else if(Input.GetAxis(ButtonNames.MoveHumanX) > 0) {
                    //Right
                    _currentAxisDelay = _axisDelay;
                    ChangeActiveButton(Direction.Right);
                }

                if(Input.GetAxis(ButtonNames.MoveHumanY) < 0) {
                    //Down
                    _currentAxisDelay = _axisDelay;
                    ChangeActiveButton(Direction.Down);
                }
                else if(Input.GetAxis(ButtonNames.MoveHumanY) > 0) {
                    //Up
                    _currentAxisDelay = _axisDelay;
                    ChangeActiveButton(Direction.Up);
                }
            }
            else {
                _currentAxisDelay -= Time.deltaTime;
            }

            if(Input.GetButtonDown(ButtonNames.HumanInteract)) {
                if(_codeSolved) return;
                _passwordText = _passwordText + _currentActiveButton.ButtonNumber;
                if(_passwordText.Length > 0 && _password[0] == _passwordText[0]) {
                    Debug.Log(_password[0] + " " + _passwordText[0]);
                    if(_passwordText.Length > 1 && _password[1] == _passwordText[1]) {
                        Debug.Log(_password[1] + " " + _passwordText[1]);
                        if(_passwordText.Length > 2 && _password[2] == _passwordText[2]) {
                            Debug.Log(_password[2] + " " + _passwordText[2]);
                            _codeSolved = true;
                        }
                        else {
                            if(_passwordText.Length > 2) {
                                _passwordText = "";
                                _gameManager.HumanController.TakeHealth(1);
                            }
                        }
                    }
                    else {
                        if(_passwordText.Length > 1) {
                            _passwordText = "";
                            _gameManager.HumanController.TakeHealth(1);
                        }
                    }
                }
                else {
                    if(_passwordText.Length > 0) {
                        _passwordText = "";
                        _gameManager.HumanController.TakeHealth(1);
                    }
                }
            }

            if(Input.GetButtonDown(ButtonNames.HumanJoystickButtonA)) {
                CloseMap();
            }
        }
        
        private void AutoClose() {
            if(!_codeSolved) return;
            _closeTimeAfterCodeSolved -= Time.deltaTime;
            if(_closeTimeAfterCodeSolved < 0 && _visibilityObject.activeInHierarchy)
                CloseMap();
        }


        private void ChangeActiveButton(Direction direction) {
            switch(direction) {
                case Direction.Up:
                    if(_currentActiveButton.ButtonOnUp != null) {
                        _currentActiveButton.ToggleActive();
                        _currentActiveButton = _currentActiveButton.ButtonOnUp;
                        _currentActiveButton.ToggleActive();
                    }

                    break;
                case Direction.Down:
                    if(_currentActiveButton.ButtonOnDown != null) {
                        _currentActiveButton.ToggleActive();
                        _currentActiveButton = _currentActiveButton.ButtonOnDown;
                        _currentActiveButton.ToggleActive();
                    }

                    break;
                case Direction.Left:
                    if(_currentActiveButton.ButtonOnLeft != null) {
                        _currentActiveButton.ToggleActive();
                        _currentActiveButton = _currentActiveButton.ButtonOnLeft;
                        _currentActiveButton.ToggleActive();
                    }

                    break;
                case Direction.Right:
                    if(_currentActiveButton.ButtonOnRight != null) {
                        _currentActiveButton.ToggleActive();
                        _currentActiveButton = _currentActiveButton.ButtonOnRight;
                        _currentActiveButton.ToggleActive();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException("direction", direction, null);
            }
        }

        public void CloseMap() {
            if(_codeSolved && !_visibilityObject.activeInHierarchy) return;
            _humanMapActive = false;
            _visibilityObject.SetActive(false);
        }

        public void OpenMap() {
            if(_codeSolved || _visibilityObject.activeSelf) return;
            Debug.Log("Test");
            _humanMapActive = true;
            _visibilityObject.SetActive(true);
            _wait = 0.5f;
        }


        public bool HumanMapActive {
            get {return _humanMapActive;}
        }

        public bool CodeSolved {
            get {return _codeSolved;}
        }

        public Sprite Highlight {
            get {return _highlight;}
        }

        public Sprite Transpa {
            get {return _transpa;}
        }
    }
}
