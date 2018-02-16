using TrustfallGames.KeepTalkingAndEscape.Listener;
using UnityEngine;

namespace TrustfallGames.KeepTalkingAndEscape.Datatypes {
    public class NumButton :MonoBehaviour {
        [SerializeField] private NumButtonHandler _numButtonHandler;
        [SerializeField] private NumButtonType _numButtonType;
        [SerializeField] private int _number;
        private bool _active;
        private Sprite _sprite;
        private float counter;

        

        private float _timer;
        private float _buttonPressedTime;

        public NumButtonType NumButtonType {
            get {return _numButtonType;}
        }

        public int Number {
            get {return _number;}
        }
        public void Active() {
            counter = 0.5f;
        }

        private void Start() {
            _sprite = gameObject.GetComponent<Sprite>();
        }

        private void Update() {
            if(_active) {
                _sprite = _numButtonHandler.Clicked;
                counter -= Time.deltaTime;
            }
            else {
                _sprite = _numButtonHandler.NotClicked;
            }
            
        }

    }
}
