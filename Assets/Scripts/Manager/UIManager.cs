using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

namespace TrustfallGames.KeepTalkingAndEscape.Manager {
    public class UIManager : MonoBehaviour {


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

        [SerializeField] private Text _ghostHoverText;
        [SerializeField] private Text _humanHoverText;
        [SerializeField] private Text _ghostFlavourText;
        [SerializeField] private Text _humanFlavourText;
        [SerializeField] private Text _healthText;
        private float _timerHuman;
        private float _timerGhost;
        [Range(1,100)][SerializeField] private float _flavourTextWaitTimer;

        private GameManager _gameManager;
        
        public static UIManager GetUiManager() {
            return GameObject.Find("UIManager").GetComponent<UIManager>();
        }

        private void Start() {
            _gameManager = GameManager.GetGameManager();
            
        }

        private void FixedUpdate() {
            UpdateFlavourText();
            UpdateHealth();
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
    }
}
