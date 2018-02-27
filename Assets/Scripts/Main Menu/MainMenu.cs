using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TrustfallGames.KeepTalkingAndEscape.Manager {
    public class MainMenu : MonoBehaviour {

        private FadeOut _fadeOut;

        [SerializeField] private GameObject[] _menu;
        [SerializeField] private String[] _scene;

        [SerializeField] private float _axisDelay = 0.5f;
        [SerializeField] private CharacterType _characterType;

        private float _currentAxisDelay;
        private static int _currentPressedButtonHuman;
        private static int _currentPressedButtonGhost;
        
        [SerializeField] private Sprite _outline;
        [SerializeField] private Sprite _defaultSprite;

        private SoundManager _soundManager;
        private int _y = 0;

        private void Start() {
            _soundManager = GameObject.Find("SoundManagerMenu").GetComponent<SoundManager>();
            _soundManager.Source.loop = false;
            _currentPressedButtonHuman = 5;
            _currentPressedButtonGhost = 5;    
        }

        // Update is called once per frame
        private void Update() {
            MainMenuInput();
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
                    if(_currentAxisDelay < 0) {
                        if(Input.GetAxis(ButtonNames.MoveHumanY) < 0) {
                            //Up
                            if(_y == 4) return;
                            _y++;
                            _currentAxisDelay = _axisDelay;
                        }
                        else if(Input.GetAxis(ButtonNames.MoveHumanY) > 0) {
                            //Down
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
                    //Change current button
                    if(_currentAxisDelay < 0) {
                        if(Input.GetAxis(ButtonNames.MoveGhostY) < 0) {
                            //Up
                            Debug.Log("Pressed Down");
                            if(_y == 4) return;
                            _y++;
                            _currentAxisDelay = _axisDelay;
                        }

                        if(Input.GetAxis(ButtonNames.MoveGhostY) > 0) {
                            //Down
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
        
        public void Clicked() {
            switch(_characterType) {
                case CharacterType.Human:
                    if(_currentAxisDelay <= 0) {
                        if(Input.GetButtonDown(ButtonNames.HumanInspect))
                            if(_menu[_y] != null) {
                                if(_menu[0]) {
                                    _currentPressedButtonHuman =_y;
                                    _currentAxisDelay = _axisDelay;
                                    Debug.Log("Pressed Human Button is number: " + _currentPressedButtonHuman + _currentPressedButtonGhost);
                                }
                                
                                if(_menu[1]) {
                                    _currentPressedButtonHuman =_y;
                                    _currentAxisDelay = _axisDelay;
                                }
                                
                                if(_menu[2]) {
                                    _currentPressedButtonHuman =_y;
                                    _currentAxisDelay = _axisDelay;
                                }
                                
                                if(_menu[3]) {
                                    _currentPressedButtonHuman =_y;
                                    _currentAxisDelay = _axisDelay;
                                }
                                
                                if(_menu[4]) {
                                    _currentPressedButtonHuman =_y;
                                    _currentAxisDelay = _axisDelay;
                                }
                            }
                    }
                    else {
                        _currentAxisDelay -= Time.deltaTime;
                    }
                    break;

                case CharacterType.Ghost:
                    if(_currentAxisDelay < 0) {
                       if(Input.GetButtonDown(ButtonNames.GhostInspect))
                           if(_menu[_y] != null) {
                               if(_menu[0]) {
                                   _currentPressedButtonGhost =_y;
                                   _currentAxisDelay = _axisDelay;
                               }
                                
                               if(_menu[1]) {
                                   _currentPressedButtonGhost =_y;
                                   _currentAxisDelay = _axisDelay;
                               }
                                
                               if(_menu[2]) {
                                   _currentPressedButtonGhost =_y;
                                   _currentAxisDelay = _axisDelay;
                               }
                                
                               if(_menu[3]) {
                                   _currentPressedButtonGhost =_y;
                                   _currentAxisDelay = _axisDelay;
                               }

                                
                               if(_menu[4]) {
                                   _currentPressedButtonGhost =_y;
                                   _currentAxisDelay = _axisDelay;
                               }

                               Debug.Log("Pressed Ghost Button is number: " + _currentPressedButtonGhost);
                           }
                    }
                    else {
                        _currentAxisDelay -= Time.deltaTime;
                    }
                    break;
            }
        }

        private void SceneManagement() {
            if(_currentPressedButtonHuman == 0 && _currentPressedButtonGhost == 0) {
                _fadeOut = GameObject.Find("Panel").AddComponent<FadeOut>();
                _currentPressedButtonHuman = 5;
                _currentPressedButtonGhost = 5;  
//                SceneManager.LoadScene(_scene[0]);
            }
            if(_currentPressedButtonHuman == 3 && _currentPressedButtonGhost == 3) {
//                _fadeIn = FadeIn.GetFadeIn();
                SceneManager.LoadScene(_scene[3]);
            }

            if(_currentPressedButtonHuman == 4 && _currentPressedButtonGhost == 4) Application.Quit();
        }
        
        private CharacterType CharacterType {
            get {return _characterType;}
        }

        private int Y {
            get {return _y;}
        }
    }
}
