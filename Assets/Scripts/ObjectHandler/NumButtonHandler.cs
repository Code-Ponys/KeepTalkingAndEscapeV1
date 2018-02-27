using System;
using TrustfallGames.KeepTalkingAndEscape.Datatypes;
using TrustfallGames.KeepTalkingAndEscape.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace TrustfallGames.KeepTalkingAndEscape.Listener {
    public class NumButtonHandler : MonoBehaviour {
        [SerializeField] private string _password;
        [SerializeField] private NumButton[] _buttonsInput;
        [SerializeField] private Text _display;
        private NumButton[,] _buttons = new NumButton[4, 3];
        private Image[,] _highlights = new Image[4, 3];
        [SerializeField] private GameObject _visibleObject;
        [SerializeField] private Sprite _highlight;
        [SerializeField] private Sprite _empty;
        [SerializeField] private Sprite _clicked;
        [SerializeField] private Sprite _notClicked;
        [SerializeField] private float _closeTimeAfterCodeSolved = 5;
        private bool _humanNumPadActive;
        private bool _codeSolved;


        private int _x;
        private int _y;

        private string _displayText;

        [SerializeField] private float _axisDelay = 0.5f;
        private float _currentAxisDelay = 0;

        private bool _uiActive;
        private bool _lastUiState;

        private void Start() {
            var gamemanager = GameManager.GetGameManager();
            var canvas = gameObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = gamemanager.HumanCamera;
            
            foreach(var button in _buttonsInput) {
                var num = button.Number;
                var type = button.NumButtonType;
                button.NumButtonHandler = gameObject.GetComponent<NumButtonHandler>();

                switch(type) {
                    case NumButtonType.Number:
                        switch(num) {
                            case 1:
                                _buttons[0, 0] = button;
                                _highlights[0, 0] = GameObject.Find(gameObject.name + "/VisibilityObject/Background/" + button.name + "/Image").GetComponent<Image>();
                                break;
                            case 2:
                                _buttons[0, 1] = button;
                                _highlights[0, 1] = GameObject.Find(gameObject.name + "/VisibilityObject/Background/" + button.name + "/Image").GetComponent<Image>();
                                break;
                            case 3:
                                _buttons[0, 2] = button;
                                _highlights[0, 2] = GameObject.Find(gameObject.name + "/VisibilityObject/Background/" + button.name + "/Image").GetComponent<Image>();
                                break;
                            case 4:
                                _buttons[1, 0] = button;
                                _highlights[1, 0] = GameObject.Find(gameObject.name + "/VisibilityObject/Background/" + button.name + "/Image").GetComponent<Image>();
                                break;
                            case 5:
                                _buttons[1, 1] = button;
                                _highlights[1, 1] = GameObject.Find(gameObject.name + "/VisibilityObject/Background/" + button.name + "/Image").GetComponent<Image>();
                                break;
                            case 6:
                                _buttons[1, 2] = button;
                                _highlights[1, 2] = GameObject.Find(gameObject.name + "/VisibilityObject/Background/" + button.name + "/Image").GetComponent<Image>();
                                break;
                            case 7:
                                _buttons[2, 0] = button;
                                _highlights[2, 0] = GameObject.Find(gameObject.name + "/VisibilityObject/Background/" + button.name + "/Image").GetComponent<Image>();
                                break;
                            case 8:
                                _buttons[2, 1] = button;
                                _highlights[2, 1] = GameObject.Find(gameObject.name + "/VisibilityObject/Background/" + button.name + "/Image").GetComponent<Image>();
                                break;
                            case 9:
                                _buttons[2, 2] = button;
                                _highlights[2, 2] = GameObject.Find(gameObject.name + "/VisibilityObject/Background/" + button.name + "/Image").GetComponent<Image>();
                                break;
                            case 0:
                                _buttons[3, 1] = button;
                                _highlights[3, 1] = GameObject.Find(gameObject.name + "/VisibilityObject/Background/" + button.name + "/Image").GetComponent<Image>();
                                break;
                        }

                        break;
                    case NumButtonType.Reset:
                        _buttons[3, 0] = button;
                        _highlights[3, 0] = GameObject.Find(gameObject.name + "/VisibilityObject/Background/" + button.name + "/Image").GetComponent<Image>();
                        break;
                    case NumButtonType.Confirm:
                        _buttons[3, 2] = button;
                        _highlights[3, 2] = GameObject.Find(gameObject.name + "/VisibilityObject/Background/" + button.name + "/Image").GetComponent<Image>();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            CloseButtonField();
        }


        private void Update() {
            if(!_visibleObject.activeInHierarchy) return;

            foreach(var image in _highlights) {
                var a = image;
                a.sprite = _empty;
            }

            _highlights[_y, _x].sprite = _highlight;
            InputUi();
            UpdateDisplay();
            AutoClose();
        }

        private void AutoClose() {
            if(!_codeSolved) return;
            _closeTimeAfterCodeSolved -= Time.deltaTime;
            if(_closeTimeAfterCodeSolved < 0 && _visibleObject.activeInHierarchy)
                CloseButtonField();
        }

        private void UpdateDisplay() {
            _display.text = _displayText;
        }

        private void InputUi() {
            //Change current choosed Item
            if(_codeSolved) return;
            if(_currentAxisDelay < 0) {
                if(Input.GetAxis(ButtonNames.MoveHumanX) < 0) {
                    //Left
                    if(_x == 0) return;
                    _x--;
                    _currentAxisDelay = _axisDelay;
                }
                else if(Input.GetAxis(ButtonNames.MoveHumanX) > 0) {
                    //Right
                    if(_x == _buttons.GetLength(1)-1) return;
                    _x++;
                    _currentAxisDelay = _axisDelay;
                }

                if(Input.GetAxis(ButtonNames.MoveHumanY) < 0) {
                    //Down
                    if(_y == _buttons.GetLength(0)-1) return;
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

            if(Input.GetButtonDown(ButtonNames.HumanInspect) && _visibleObject.activeSelf) {
                CloseButtonField();
            }

            if(Input.GetButtonDown(ButtonNames.HumanJoystickButtonA)) {
                CloseButtonField();
            }
            
            if(!Input.GetButtonDown(ButtonNames.HumanInteract)) return;
            _buttons[_y, _x].GetComponent<NumButton>().Active();
            switch(_buttons[_y, _x].GetComponent<NumButton>().NumButtonType) {
                case NumButtonType.Number:
                    if(_displayText == "Korrekt" || _displayText == "Falsch") _displayText = "";

                    _displayText = _displayText + _buttons[_y, _x].GetComponent<NumButton>().Number;
                    if(_displayText.Length == _password.Length)
                        if(string.Equals(_displayText, _password, StringComparison.CurrentCultureIgnoreCase)) {
                            _displayText = "Korrekt";
                            _codeSolved = true;
                        }
                        else {
                            _displayText = "Falsch";
                        }

                    break;
                case NumButtonType.Reset:
                    _displayText = "";
                    break;
                case NumButtonType.Confirm:
                    _displayText = "Falsch";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void CloseButtonField() {
            if(_codeSolved && !_visibleObject.activeInHierarchy) return;
            _humanNumPadActive = false;
            _visibleObject.SetActive(false);
        }

        public void OpenButtonField() {
            if(_codeSolved) return;
            _humanNumPadActive = true;
            _visibleObject.SetActive(true);
        }

        private void OnEnable() {
            _displayText = "";
        }

        public Sprite Clicked {
            get {return _clicked;}
            set {_clicked = value;}
        }

        public Sprite NotClicked {
            get {return _notClicked;}
            set {_notClicked = value;}
        }

        public bool CodeSolved {
            get {return _codeSolved;}
        }
        
        public bool HumanNumPadActive {
            get {return _humanNumPadActive;}
        }
    }
}
