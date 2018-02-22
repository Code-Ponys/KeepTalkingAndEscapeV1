using System;
using System.Collections.Generic;
using TrustfallGames.KeepTalkingAndEscape.Datatypes;
using TrustfallGames.KeepTalkingAndEscape.Manager;
using UnityEngine;

namespace TrustfallGames.KeepTalkingAndEscape.Listener {
    public class MapHandler {
        private List<MapButton> _buttons = new List<MapButton>();
        private float _currentAxisDelay;
        [SerializeField] private string _password;
        [SerializeField] private GameObject _visibilityObject;
        private string _passwordText;

        private MapButton _currentActiveButton;
        private bool _codeSolved;

        private GameManager _gameManager;
        private bool _humanMapActive;

        public void RegisterButton(MapButton button) {
            _buttons.Add(button);
            _gameManager = GameManager.GetGameManager();
        }


        public void InputMap() {
            if(_currentAxisDelay < 0) {
                if(Input.GetAxis(ButtonNames.MoveHumanX) < 0) {
                    //Left
                    ChangeActiveButton(Direction.Left);
                }
                else if(Input.GetAxis(ButtonNames.MoveHumanX) > 0) {
                    //Right
                    ChangeActiveButton(Direction.Right);
                }

                if(Input.GetAxis(ButtonNames.MoveHumanY) < 0) {
                    //Down
                    ChangeActiveButton(Direction.Down);
                }
                else if(Input.GetAxis(ButtonNames.MoveHumanY) > 0) {
                    //Up
                    ChangeActiveButton(Direction.Up);
                }
            }
            else {
                _currentAxisDelay -= Time.deltaTime;
            }

            if(Input.GetButtonDown(ButtonNames.HumanInspect)) {
                if(_codeSolved) return;
                _passwordText = _passwordText + _currentActiveButton.ButtonNumber;
                if(_passwordText.Length > 0 && _password[0] == _passwordText[0]) {
                    if(_passwordText.Length > 1 && _password[1] == _passwordText[1]) {
                        if(_passwordText.Length > 2 &&_password[2] == _passwordText[2]) {
                            _codeSolved = true;
                        }
                        else {
                            _passwordText = "";
                            _gameManager.HumanController.TakeHealth(1);
                        }
                    }
                    else {
                        _passwordText = "";
                        _gameManager.HumanController.TakeHealth(1);
                    }
                }
                else {
                    _passwordText = "";
                    _gameManager.HumanController.TakeHealth(1);
                }
            }
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
                    if(_currentActiveButton.ButtonOnUp != null) {
                        _currentActiveButton.ToggleActive();
                        _currentActiveButton = _currentActiveButton.ButtonOnUp;
                        _currentActiveButton.ToggleActive();
                    }

                    break;
                case Direction.Left:
                    if(_currentActiveButton.ButtonOnUp != null) {
                        _currentActiveButton.ToggleActive();
                        _currentActiveButton = _currentActiveButton.ButtonOnUp;
                        _currentActiveButton.ToggleActive();
                    }

                    break;
                case Direction.Right:
                    if(_currentActiveButton.ButtonOnUp != null) {
                        _currentActiveButton.ToggleActive();
                        _currentActiveButton = _currentActiveButton.ButtonOnUp;
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
            if(_codeSolved) return;
            _humanMapActive = true;
            _visibilityObject.SetActive(true);
        }

        public bool HumanMapActive {
            get {return _humanMapActive;}
        }

        public bool CodeSolved {
            get {return _codeSolved;}
        }
    }
}
