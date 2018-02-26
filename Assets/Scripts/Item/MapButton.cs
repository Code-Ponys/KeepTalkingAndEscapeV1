using TrustfallGames.KeepTalkingAndEscape.Listener;
using UnityEngine;
using UnityEngine.UI;

namespace TrustfallGames.KeepTalkingAndEscape.Datatypes {
    public class MapButton : MonoBehaviour {
        [SerializeField] private MapHandler _mapHandler;
        [SerializeField] private string _buttonNumber;
        [SerializeField] private MapButton _ButtonOnLeft;
        [SerializeField] private MapButton _ButtonOnRight;
        [SerializeField] private MapButton _ButtonOnUp;
        [SerializeField] private MapButton _ButtonOnDown;
        private Image _highlight;


        private bool _active;


        private void Start() {
            _mapHandler.RegisterButton(this);
            _highlight = GameObject.Find(gameObject.name + "/Image").GetComponent<Image>();

        }

        private void Update() {
            if(_active) {
                _highlight.sprite = _mapHandler.Highlight;
            }
            else {
                _highlight.sprite = _mapHandler.Transpa;
            }
        }

        public void ToggleActive() {
            _active = !_active;
        }

        public string ButtonNumber {
            get {return _buttonNumber;}
        }

        public MapButton ButtonOnLeft {
            get {return _ButtonOnLeft;}
        }

        public MapButton ButtonOnRight {
            get {return _ButtonOnRight;}
        }

        public MapButton ButtonOnUp {
            get {return _ButtonOnUp;}
        }

        public MapButton ButtonOnDown {
            get {return _ButtonOnDown;}
        }

    }
}
