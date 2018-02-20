using TrustfallGames.KeepTalkingAndEscape.Listener;
using UnityEngine;
using UnityEngine.UI;

namespace TrustfallGames.KeepTalkingAndEscape.Datatypes {
    public class NumButton : MonoBehaviour {
        private NumButtonHandler _numButtonHandler;
        [SerializeField] private NumButtonType _numButtonType;
        [SerializeField] private int _number;
        private Image _sprite;
        private float _counter;


        private float _timer;
        private float _buttonPressedTime;

        public NumButtonType NumButtonType {
            get {return _numButtonType;}
        }

        public int Number {
            get {return _number;}
        }

        public void Active() {
            _counter = 0.5f;
        }

        private void Start() {
            _sprite = gameObject.GetComponent<Image>();
        }

        private void Update() {
            if(_numButtonHandler == null)
                return;

            if(_counter > 0) {
                _sprite.sprite = _numButtonHandler.Clicked;
                _counter -= Time.deltaTime;
            }
            else {
                _sprite.sprite = _numButtonHandler.NotClicked;
            }
        }

        public NumButtonHandler NumButtonHandler {
            get {return _numButtonHandler;}
            set {_numButtonHandler = value;}
        }
        
    }
}
