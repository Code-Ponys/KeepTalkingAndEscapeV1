﻿using System;
using TrustfallGames.KeepTalkingAndEscape.Datatypes;
using TrustfallGames.KeepTalkingAndEscape.Listener;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

namespace TrustfallGames.KeepTalkingAndEscape.Manager {
    public class UIManager : MonoBehaviour {

        [SerializeField] private Text _ghostHoverText;
        [SerializeField] private Text _humanHoverText;
        [SerializeField] private Text _ghostFlavourText;
        [SerializeField] private Text _humanFlavourText;
        [SerializeField] private Text _healthText;

        [SerializeField] private Inventory _inventoryGhost;
        [SerializeField] private Inventory _inventoryHuman;

        [SerializeField] private Canvas _userInterfaceGhost;
        [SerializeField] private Canvas _userInterfaceHuman;

        [SerializeField] private Image _ItemInHand;
        [SerializeField] private Text _ItemInHandText;
        [SerializeField] private Image _itemDisplayGhost;
        [SerializeField] private Text _itemDisplayGhostText;

        [SerializeField] private Image _ghostFirstButton;
        [SerializeField] private Image _ghostSecondButton;
        [SerializeField] private Text _ghostFirstButtonText;
        [SerializeField] private Text _ghostSecondButtonText;

        [SerializeField] private Image _humanFirstButton;
        [SerializeField] private Image _humanSecondButton;
        [SerializeField] private Text _humanFirstButtonText;
        [SerializeField] private Text _humanSecondButtonText;

        //MenuGhost
        [SerializeField] private StartMenu _menuGhost;
        [SerializeField] private Image _ghostContinue;
        [SerializeField] private Text _ghostContinueText;
        [SerializeField] private Image _ghostReplay;
        [SerializeField] private Text _ghostReplayText;
        [SerializeField] private Image _ghostQuit;
        [SerializeField] private Text _ghostQuitText;

        //MenuHuman
        [SerializeField] private StartMenu _menuHuman;
        [SerializeField] private Image _humanContinue;
        [SerializeField] private Text _humanContinueText;
        [SerializeField] private Image _humanReplay;
        [SerializeField] private Text _humanReplayText;
        [SerializeField] private Image _humanQuit;
        [SerializeField] private Text _humanQuitText;

        [SerializeField] private Sprite _a;
        [SerializeField] private Sprite _b;
        [SerializeField] private Sprite _x;
        [SerializeField] private Sprite _y;
        [SerializeField] private Sprite _start;
        [SerializeField] private Sprite _back;
        [SerializeField] private Sprite _rb;
        [SerializeField] private Sprite _lb;
        [SerializeField] private Sprite _rt;
        [SerializeField] private Sprite _lt;
        [SerializeField] private Sprite _pad;
        [SerializeField] private Sprite _l;
        [SerializeField] private Sprite _r;

        [SerializeField] private Sprite _transparent;

        [SerializeField] private ImageCanvasHandler _canvasHandlerHuman;
        [SerializeField] private ImageCanvasHandler _canvasHandleGhost;

        private int _instanceIdHuman;
        private int _instanceIdGhost;

        private float _timerHuman;
        private float _timerGhost;

        private float _pickUpTimerGhost;
        private float _pickUpTimerHuman;

        private SoundManager _soundManager;

        [Range(1, 100)] [SerializeField] private float _flavourTextWaitTimer;
        [Range(1, 100)] [SerializeField] private float _pickUpDisplayTimer;

        private GameManager _gameManager;
        private ItemManager _itemManager;

        private bool _deathSoundPlayed;

        public static UIManager GetUiManager() {
            if(GameObject.Find("System") == null) return null;
            return GameObject.Find("System").GetComponent<UIManager>();
        }

        private void Start() {
            _gameManager = GameManager.GetGameManager();
            _itemManager = ItemManager.GetItemManager();
            _soundManager = SoundManager.GetSoundManager();

            _userInterfaceGhost.renderMode = RenderMode.ScreenSpaceCamera;
            _userInterfaceHuman.renderMode = RenderMode.ScreenSpaceCamera;
            _userInterfaceGhost.worldCamera = _gameManager.GhostCamera;
            _userInterfaceHuman.worldCamera = _gameManager.HumanCamera;

            ClearUI();

            TriggerMenu(false,CharacterType.Ghost);
            TriggerMenu(false,CharacterType.Human);
            
            TriggerGameOverScreen(false);
        }

        private void ClearUI() {
            _ghostFirstButton.sprite = GetSprite(KeyType.none);
            _ghostSecondButton.sprite = GetSprite(KeyType.none);
            _humanFirstButton.sprite = GetSprite(KeyType.none);
            _humanSecondButton.sprite = GetSprite(KeyType.none);

            _ghostFirstButtonText.text = "";
            _ghostSecondButtonText.text = "";
            _humanFirstButtonText.text = "";
            _humanSecondButtonText.text = "";
            _humanHoverText.text = "";
            _ghostHoverText.text = "";
            _humanFlavourText.text = "";
            _ghostFlavourText.text = "";
        }

        private void FixedUpdate() {
            UpdateFlavourText();
            UpdateHealth();
            UpdateItemInHand();
            
            if(_gameManager.HumanController.Health <= 0 && !_deathSoundPlayed) {
                _soundManager.Source.clip = _soundManager.DeathSound;
                _soundManager.Source.Play();
                _deathSoundPlayed = true;
                _soundManager.Source.loop = true;
            }
        }

        private void UpdateItemInHand() {
            if(_inventoryHuman.ItemInHand != "") {
                var item = _itemManager.GetItemFromDatabase(_inventoryHuman.ItemInHand);
                _ItemInHand.sprite = Resources.Load<Sprite>(item.SpritePath);
                _ItemInHandText.text = item.Name;
                return;
            }

            if(_pickUpTimerGhost > 0) {
                _pickUpTimerGhost -= Time.deltaTime;
            }
            else {
                _itemDisplayGhost.sprite = _transparent;
                _itemDisplayGhostText.text = "";
            }


            if(_pickUpTimerHuman > 0) {
                _pickUpTimerHuman -= Time.deltaTime;
            }
            else {
                _ItemInHandText.text = "";
                _ItemInHand.sprite = _transparent;
            }
        }

        public void DisplayLastPickedUpItem(Item item) {
            var sprite = Resources.Load<Sprite>(item.SpritePath);
            _ItemInHand.sprite = sprite;
            _itemDisplayGhost.sprite = sprite;
            _ItemInHandText.text = item.Name + " aufgehoben";
            _itemDisplayGhostText.text = item.Name + " aufgehoben";
            _pickUpTimerGhost = _pickUpDisplayTimer;
            _pickUpTimerHuman = _pickUpDisplayTimer;
        }

        private void UpdateHealth() {
            _healthText.text = "Gesundheit: " + _gameManager.Human.GetComponent<FirstPersonControllerHuman>().Health;
        }

        private void UpdateFlavourText() {
            if(_humanFlavourText.text != "") {
                _timerHuman += Time.deltaTime;
                if(_timerHuman > _flavourTextWaitTimer) {
                    _humanFlavourText.text = "";
                    _timerHuman = 0;
                }
            }

            if(_ghostFlavourText.text != "") {
                _timerGhost += Time.deltaTime;
                if(_timerGhost > _flavourTextWaitTimer) {
                    _ghostFlavourText.text = "";
                    _timerGhost = 0;
                }
            }
        }

        public void ShowButtons(CharacterType type, KeyType firstButton, KeyType secondButton, int instanceId) {
            switch(type) {
                case CharacterType.Unassigned:
                    break;
                case CharacterType.Ghost:
                    _instanceIdGhost = instanceId;
                    _ghostFirstButton.sprite = GetSprite(firstButton);
                    switch(firstButton) {
                        case KeyType.A:
                            _ghostFirstButtonText.text = "Untersuchen";
                            break;
                        case KeyType.B:
                            _ghostFirstButtonText.text = "Interagieren";
                            break;
                        case KeyType.none:
                            _ghostFirstButtonText.text = "";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("firstButton", firstButton, null);
                    }

                    _ghostSecondButton.sprite = GetSprite(secondButton);
                    switch(secondButton) {
                        case KeyType.A:
                            _ghostSecondButtonText.text = "Untersuchen";
                            break;
                        case KeyType.none:
                            _ghostSecondButtonText.text = "";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("secondButton", secondButton, null);
                    }

                    break;
                case CharacterType.Human:
                    _instanceIdHuman = instanceId;
                    _humanFirstButton.sprite = GetSprite(firstButton);
                    switch(firstButton) {
                        case KeyType.B:
                            _humanFirstButtonText.text = "Interagieren";
                            break;
                        case KeyType.none:
                            _humanFirstButtonText.text = "";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("firstButton", firstButton, null);
                    }

                    _humanSecondButton.sprite = GetSprite(secondButton);
                    switch(secondButton) {
                        case KeyType.A:
                            _humanSecondButtonText.text = "Untersuchen";
                            break;
                        case KeyType.none:
                            _humanSecondButtonText.text = "";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("secondButton", secondButton, null);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }
        }


        /// <summary>
        /// Hides Buttons and Hover Text
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void HideButtons(CharacterType type) {
            ShowButtons(type, KeyType.none, KeyType.none, 0);
            switch(type) {
                case CharacterType.Unassigned:
                    break;
                case CharacterType.Ghost:
                    _ghostHoverText.text = "";
                    break;
                case CharacterType.Human:
                    _humanHoverText.text = "";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }
        }

        public void ClearUI(CharacterType characterType) {
            HideButtons(characterType);
            
        }

        private void HideTexts(CharacterType characterType) {
            switch(characterType) {
                case CharacterType.Ghost:
                    _ghostHoverText.text = "";
                    break;
                case CharacterType.Human:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("characterType", characterType, null);
            }
        }
        

        private void UpdateHumanUI() {
            if(_currentInteractionListenerHuman == null) {
                HideButtons(CharacterType.Human);
                return;
            }

            //Show text if the object is inside the view distance
            HumanHoverText = _currentInteractionListenerHuman.ObjectDescription;

            if(!_humanReachable) return;

            if(_objectDescriptionHuman.Equals("")) {
                ShowButtons(CharacterType.Human, KeyType.A, KeyType.none);
                return;
            }

            if(!_objectDescriptionHuman.Equals(""))
                ShowButtons(CharacterType.Human, KeyType.A, KeyType.B);
        }

        private void UpdateGhostUI() {
            //Show Keys while in ghost animation
            if(_gameManager.GhostDrivenAnimationActive) {
                ShowButtonsAnimation(CharacterType.Ghost, _currentInteractionListenerGhost.KeyType, KeyType.B);
                return;
            }

            if(_currentInteractionListenerGhost == null) {
                HideButtons(CharacterType.Ghost);
                return;
            }

            //Show text if the object is inside the view distance
            GhostHoverText = _objectDescriptionGhost;

            if(!_ghostReachable) return;

            if(_currentInteractionListenerGhost.ActivateObjectWithGhostInteraction && _currentInteractionListenerGhost.GhostFlavourText.Equals("")) {
                ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.none);
                return;
            }

            if(_currentInteractionListenerGhost.ActivateObjectWithGhostInteraction && !_currentInteractionListenerGhost.GhostFlavourText.Equals("")) {
                ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.A);
                return;
            }


            if(_currentInteractionListenerGhost.GhostFlavourText.Equals("") && _ghostCanOpen) {
                ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.none);
                return;
            }

            if(!_ghostCanOpen && !_currentInteractionListenerGhost.GhostFlavourText.Equals("")
                              && _currentInteractionListenerGhost.ShowImageOnInteraction
                              && !_currentInteractionListenerGhost.AnimationUnlocked) {
                ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.none);
                return;
            }

            if(!_ghostCanOpen && !_currentInteractionListenerGhost.GhostFlavourText.Equals("")
                              && _currentInteractionListenerGhost.ShowImageOnInteraction
                              && _currentInteractionListenerGhost.AnimationUnlocked) {
                ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.A);
                return;
            }

            if(!_ghostCanOpen && !_currentInteractionListenerGhost.GhostFlavourText.Equals("")) {
                ShowButtons(CharacterType.Ghost, KeyType.A, KeyType.none);
                return;
            }

            if(_ghostCanOpen && !_currentInteractionListenerGhost.GhostFlavourText.Equals("")) {
                ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.A);
            }
        }


        public Sprite A {
            get {return _a;}
        }

        public Sprite B {
            get {return _b;}
        }

        public Sprite X {
            get {return _x;}
        }

        public Sprite Y {
            get {return _y;}
        }

        public Sprite Start1 {
            get {return _start;}
        }

        public Sprite Back {
            get {return _back;}
        }

        public Sprite Rb {
            get {return _rb;}
        }

        public Sprite Lb {
            get {return _lb;}
        }

        public Sprite Rt {
            get {return _rt;}
        }

        public Sprite Lt {
            get {return _lt;}
        }

        public Sprite Pad {
            get {return _pad;}
        }

        public Sprite L {
            get {return _l;}
        }

        public Sprite R {
            get {return _r;}
        }


        public void ShowButtonsAnimation(CharacterType ghost, KeyType firstButton, KeyType secondButton) {
            _ghostFirstButtonText.text = "Bewegen";
            _ghostSecondButtonText.text = "Abbrechen";
            _ghostFirstButton.sprite = GetSprite(firstButton);
            _ghostSecondButton.sprite = GetSprite(secondButton);
        }

        public void ShowImage(CharacterType characterType, Sprite sprite) {
            switch(characterType) {
                case CharacterType.Unassigned:
                    break;
                case CharacterType.Ghost:
                    _canvasHandleGhost.ShowImage(sprite);
                    break;
                case CharacterType.Human:
                    _canvasHandlerHuman.ShowImage(sprite);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("characterType", characterType, null);
            }
        }


        private Sprite GetSprite(KeyType key) {
            switch(key) {
                case KeyType.X:
                    return X;
                case KeyType.Y:
                    return Y;
                case KeyType.A:
                    return A;
                case KeyType.B:
                    return B;
                case KeyType.R1:
                    return Rb;
                case KeyType.R2:
                    return Rt;
                case KeyType.L1:
                    return Lb;
                case KeyType.L2:
                    return Lt;
                case KeyType.none:
                    return _transparent;
                default:
                    throw new ArgumentOutOfRangeException("key", key, null);
            }
        }

        public void TriggerMenu(bool state, CharacterType characterType) {
            switch(characterType) {
                case CharacterType.Unassigned:
                    break;
                case CharacterType.Ghost:
                    _ghostContinue.enabled = state;
                    _ghostContinueText.enabled = state;
                    _ghostReplay.enabled = state;
                    _ghostReplayText.enabled = state;
                    _ghostQuit.enabled = state;
                    _ghostQuitText.enabled = state;
                    break;
                case CharacterType.Human:
                    _humanContinue.enabled = state;
                    _humanContinueText.enabled = state;
                    _humanReplay.enabled = state;
                    _humanReplayText.enabled = state;
                    _humanQuit.enabled = state;
                    _humanQuitText.enabled = state;

                    break;
                default:
                    throw new ArgumentOutOfRangeException("characterType", characterType, null);
            }
        }

        public void SetCurrentDisplayObject(ObjectInteractionListener interactionListener, bool reachable, CharacterType characterType) {
            switch(characterType) {
                case CharacterType.Ghost:
                    _currentInteractionListenerGhost = interactionListener;
                    _ghostReachable = reachable;
                    _ghostCanOpen = interactionListener.GhostCanOpen;
                    _objectDescriptionGhost = interactionListener.ObjectDescription;
                    break;
                case CharacterType.Human:
                    _currentInteractionListenerHuman = interactionListener;
                    _humanReachable = reachable;
                    _objectDescriptionHuman = interactionListener.ObjectDescription;

                    break;
                default:
                    throw new ArgumentOutOfRangeException("characterType", characterType, null);
            }
        }
        

        public string GhostHoverText {
            get {return _ghostHoverText.text;}
            set {_ghostHoverText.text = value;}
        }

        public string HumanHoverText {
            get {return _humanHoverText.text;}
            set {_humanHoverText.text = value;}
        }

        public string GhostFlavourText {
            get {return _ghostFlavourText.text;}
            set {
                _ghostFlavourText.text = value;
                _timerGhost = 0;
            }
        }

        public string HumanFlavourText {
            get {return _humanFlavourText.text;}
            set {
                _humanFlavourText.text = value;
                _timerHuman = 0;
            }
        }

        public string HealthText {
            get {return _healthText.text;}
            set {_healthText.text = value;}
        }

        public Inventory InventoryGhost {
            get {return _inventoryGhost;}
        }

        public Inventory InventoryHuman {
            get {return _inventoryHuman;}
        }

        public Sprite Transparent {
            get {return _transparent;}
        }

    }
}
