using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TrustfallGames.KeepTalkingAndEscape.Manager {
    public class Credits : MonoBehaviour {

        [SerializeField] private GameObject[] _menu;
        [SerializeField] private string[] _scene;

        [SerializeField] private CharacterType _characterType;
        private static int _currentPressedButtonHuman;
        private static int _currentPressedButtonGhost;

        [SerializeField] private Sprite _outline;
        [SerializeField] private Sprite _defaultSprite;

        private int _y = 0;

        // Use this for initialization
        void Start() {
            _currentPressedButtonHuman = 5;
            _currentPressedButtonGhost = 5;
        }

        // Update is called once per frame
        void Update() {
            UpdateSelection();
            Clicked();
            SceneManagement();
        }

        private void UpdateSelection() {
            foreach(var obj in _menu) obj.GetComponent<Image>().sprite = _defaultSprite;

            foreach(var obj in _menu) obj.GetComponent<Image>().sprite = _defaultSprite;
            switch(_characterType) {
                case CharacterType.Ghost:
                    _menu[_y].GetComponent<Image>().sprite = _outline;
                    break;
                case CharacterType.Human:
                    _menu[_y].GetComponent<Image>().sprite = _outline;
                    break;
            }
        }

        private void MainMenuInput() {
            //Button input for human
            switch(_characterType) {
                case CharacterType.Human:
                    //Change current button
                    if(Input.GetAxis(ButtonNames.MoveHumanY) < 0) {
                        //Up
                        if(_y == 1) return;
                        _y++;
                    }
                    else if(Input.GetAxis(ButtonNames.MoveHumanY) > 0) {
                        //Down
                        if(_y == 0) return;
                        _y--;
                    }

                    break;
                case CharacterType.Ghost:
                    //Change current button
                    if(Input.GetAxis(ButtonNames.MoveGhostY) < 0) {
                        //Up
                        Debug.Log("Pressed Down");
                        if(_y == 1) return;
                        _y++;
                    }

                    if(Input.GetAxis(ButtonNames.MoveGhostY) > 0) {
                        //Down
                        Debug.Log("Pressed Up");
                        if(_y == 0) return;
                        _y--;
                    }

                    break;
            }
        }

        public void Clicked() {
            switch(_characterType) {
                case CharacterType.Human:
                    if(Input.GetButtonDown(ButtonNames.HumanInspect))
                        if(_menu[_y] != null) {
                            if(_menu[0]) {
                                _currentPressedButtonHuman = _y;
                            }
                        }

                    break;

                case CharacterType.Ghost:
                    if(Input.GetButtonDown(ButtonNames.GhostInspect))
                        if(_menu[_y] != null) {
                            if(_menu[0]) {
                                _currentPressedButtonGhost = _y;
                            }
                        }

                    break;
            }
        }

        private void SceneManagement() {
            if(_currentPressedButtonHuman == 0 && _currentPressedButtonGhost == 0) {
                _currentPressedButtonHuman = 5;
                _currentPressedButtonGhost = 5;
                SceneManager.LoadScene(_scene[0]);
            }
        }
    }
}
