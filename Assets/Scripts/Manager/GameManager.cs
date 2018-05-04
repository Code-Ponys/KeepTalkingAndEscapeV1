using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace TrustfallGames.KeepTalkingAndEscape.Manager {
    public class GameManager : MonoBehaviour {
        


        [SerializeField] private Camera _ghostCamera;
        [SerializeField] private CharacterController _ghost;
        [SerializeField] private FirstPersonControllerGhost _ghostController;
        [SerializeField] private Camera _humanCamera;
        [SerializeField] private CharacterController _human;
        [SerializeField] private FirstPersonControllerHuman _humanController;
        private bool _ghostDrivenAnimationActive;
        private bool _humanNumPadActive;
        private bool _humanMapActive;
        private bool _humanImageCanvasActive;
        private bool _ghostImageCanvasActive;
        [SerializeField] private float _humanHeight = 330;
        [SerializeField] private float _ghostHeight = 450;
        [SerializeField] private PlayerInteractionHandler _playerInteractionHandlerGhost;
        [SerializeField] private PlayerInteractionHandler _playerInteractionHandlerHuman;

        public static GameManager GetGameManager() {
            return GameObject.Find("System").GetComponent<GameManager>();
        }

        private void Start() {

        }

        private void Update() {
            if(GameObject.Find("SoundManagerMenu") != null) Destroy(GameObject.Find("SoundManagerMenu"));
        }

        public CharacterController GetCharacterController(CharacterType characterType) {
            if(characterType == CharacterType.Ghost)
                return _ghost;
            if(characterType == CharacterType.Human)
                return _human;
        }

        public bool HumanMapActive {
            get {return _humanMapActive;}
            set {_humanMapActive = value;}
        }
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

        public FirstPersonControllerGhost GhostController {
            get {return _ghostController;}
        }

        public FirstPersonControllerHuman HumanController {
            get {return _humanController;}
        }

        public float HumanHeight {
            get {return _humanHeight;}
        }

        public float GhostHeight {
            get {return _ghostHeight;}
        }

        public bool HumanImageCanvasActive {
            get {return _humanImageCanvasActive;}
            set {_humanImageCanvasActive = value;}
        }

        public bool GhostImageCanvasActive {
            get {return _ghostImageCanvasActive;}
            set {_ghostImageCanvasActive = value;}
        }

        public PlayerInteractionHandler PlayerInteractionHandlerGhost {
            get {return _playerInteractionHandlerGhost;}
        }

        public PlayerInteractionHandler PlayerInteractionHandlerHuman {
            get {return _playerInteractionHandlerHuman;}
        }
    }
}
