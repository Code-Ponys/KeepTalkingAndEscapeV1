using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace TrustfallGames.KeepTalkingAndEscape.Manager {
    public class GameManager : MonoBehaviour {
        public Camera GhostCamera {
            get {return _ghostCamera;}
        }

        public Camera HumanCamera {
            get {return _humanCamera;}
        }

        public bool GhostDrivenAnimationActive {
            get {return _ghostDrivenAnimationActive;}
            set {_ghostDrivenAnimationActive = value;}
        }

        public bool HumanNumPadActive {
            get {return _humanNumPadActive;}
            set {_humanNumPadActive = value;}
        }

        public CharacterController Ghost {
            get {return _ghost;}
        }

        public CharacterController Human {
            get {return _human;}
        }
        [SerializeField] private Camera _ghostCamera;
        [SerializeField] private CharacterController _ghost;
        [SerializeField] private FirstPersonControllerGhost _ghostController;
        [SerializeField] private Camera _humanCamera;
        [SerializeField] private CharacterController _human;
        [SerializeField] private FirstPersonControllerHuman _humanController;
        [SerializeField] private bool _ghostDrivenAnimationActive;
        [SerializeField] private bool _humanNumPadActive;

        public static GameManager GetGameManager() {
            return GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }
}
