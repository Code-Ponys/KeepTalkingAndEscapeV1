using TrustfallGames.KeepTalkingAndEscape.Listener;
using UnityEngine;

namespace TrustfallGames.KeepTalkingAndEscape.Datatypes {
    public class MapButton : MonoBehaviour {
        [SerializeField] private MapHandler _mapHandler;
        [SerializeField] private int _buttonNumber;
        [SerializeField] private MapButton _ButtonOnLeft;
        [SerializeField] private MapButton _ButtonOnRight;
        [SerializeField] private MapButton _ButtonOnUp;
        [SerializeField] private MapButton _ButtonOnDown;

        private bool active;


        private void Start() {
            _mapHandler.RegisterButton(this);
        }

        public void ToggleActive() {
            active = !active;
        }

        public int ButtonNumber {
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
