using System;
using System.Security;
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

        [SerializeField] private Text _humanGameOver;
        [SerializeField] private Image _humanMainMenuButton;
        [SerializeField] private Image _humanReplayButton;
        [SerializeField] private Image _humanQuitMenu;

        [SerializeField] private Text _ghostGameOver;
        [SerializeField] private Image _ghostMainMenuButton;
        [SerializeField] private Image _ghostReplayButton;
        [SerializeField] private Image _ghostQuitMenu;

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

        private int _instanceIdHuman;
        private int _instanceIdGhost;

        private float _timerHuman;
        private float _timerGhost;

        private float _pickUpTimerGhost;
        private float _pickUpTimerHuman;

        [Range(1, 100)] [SerializeField] private float _flavourTextWaitTimer;
        [Range(1, 100)] [SerializeField] private float _pickUpDisplayTimer;

        private GameManager _gameManager;
        private ItemManager _itemManager;

        public static UIManager GetUiManager() {
            return GameObject.Find("System").GetComponent<UIManager>();
        }

        private void Start() {
            _gameManager = GameManager.GetGameManager();
            _itemManager = ItemManager.GetItemManager();
            _userInterfaceGhost.renderMode = RenderMode.ScreenSpaceCamera;
            _userInterfaceHuman.renderMode = RenderMode.ScreenSpaceCamera;
            _userInterfaceGhost.worldCamera = _gameManager.GhostCamera;
            _userInterfaceHuman.worldCamera = _gameManager.HumanCamera;
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
            _humanGameOver.GetComponent<Text>().enabled = false;
            _humanMainMenuButton.GetComponent<Image>().enabled = false;
            _humanReplayButton.GetComponent<Image>().enabled = false;
            _humanQuitMenu.GetComponent<Image>().enabled = false;
            _ghostGameOver.GetComponent<Text>().enabled = false;
            _ghostMainMenuButton.GetComponent<Image>().enabled = false;
            _ghostReplayButton.GetComponent<Image>().enabled = false;
            _ghostQuitMenu.GetComponent<Image>().enabled = false;
        }

        private void FixedUpdate() {
            UpdateFlavourText();
            UpdateHealth();
            UpdateItemInHand();
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
        public void HideButtons(CharacterType type) {
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

        public int GetLastInstanceId(CharacterType characterType) {
            switch(characterType) {
                case CharacterType.Ghost:
                    return _instanceIdGhost;
                case CharacterType.Human:
                    return _instanceIdHuman;
                default:
                    throw new ArgumentOutOfRangeException("characterType", characterType, null);
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

        public Text HumanGameOver {
            get {return _humanGameOver;}
            set {_humanGameOver = value;}
        }

        public Image HumanMainMenuButton {
            get {return _humanMainMenuButton;}
            set {_humanMainMenuButton = value;}
        }

        public Image HumanReplayButton {
            get {return _humanReplayButton;}
            set {_humanReplayButton = value;}
        }

        public Image HumanQuitMenu {
            get {return _humanQuitMenu;}
            set {_humanQuitMenu = value;}
        }

        public Text GhostGameOver {
            get {return _ghostGameOver;}
            set {_ghostGameOver = value;}
        }

        public Image GhostMainMenuButton {
            get {return _ghostMainMenuButton;}
            set {_ghostMainMenuButton = value;}
        }

        public Image GhostReplayButton {
            get {return _ghostReplayButton;}
            set {_ghostReplayButton = value;}
        }

        public Image GhostQuitMenu {
            get {return _ghostQuitMenu;}
            set {_ghostQuitMenu = value;}
        }
    }
}
