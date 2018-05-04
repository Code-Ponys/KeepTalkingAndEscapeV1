﻿using System;
using TrustfallGames.KeepTalkingAndEscape.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace TrustfallGames.KeepTalkingAndEscape.Listener {
    public class ImageCanvasHandler : MonoBehaviour {
        private GameManager _gameManager;
        private UIManager _uiManager;
        private Image _image;
        private GameObject _buttonDisplay;
        [SerializeField] private CharacterType _characterType;
        [SerializeField] private bool _canvasActive;


        private void Start() {
            
            _uiManager = UIManager.GetUiManager();
            _gameManager = GameManager.GetGameManager();
            _image = GameObject.Find(gameObject.name + "/Image").GetComponent<Image>();
            _buttonDisplay = GameObject.Find(gameObject.name + "/Buttonsdisplay");
            if(_canvasActive) return;
            _image.sprite = _uiManager.Transparent;
            _buttonDisplay.SetActive(false);
        }

        private void Update() {
            switch(_characterType) {
                case CharacterType.Unassigned:
                    break;
                case CharacterType.Ghost:
                    _gameManager.GhostImageCanvasActive = _canvasActive;
                    if(Input.GetButtonDown(ButtonNames.GhostJoystickButtonA)) HideCanvas();

                    break;
                case CharacterType.Human:
                    _gameManager.HumanImageCanvasActive = _canvasActive;

                    if(Input.GetButtonDown(ButtonNames.HumanJoystickButtonA)) HideCanvas();

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        private void HideCanvas() {
            _image.sprite = _uiManager.Transparent;
            _buttonDisplay.SetActive(false);
            _canvasActive = false;
        }

        public void ShowImage(Sprite sprite) {
            _image.sprite = sprite;
            _buttonDisplay.SetActive(true);
            _canvasActive = true;
        }
    }
}
