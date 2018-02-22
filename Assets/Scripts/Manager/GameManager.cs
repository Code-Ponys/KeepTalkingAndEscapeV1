using System;
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
        [SerializeField] private bool _ghostDrivenAnimationActive;
        [SerializeField] private bool _humanNumPadActive;
        [SerializeField] private bool _humanMapActive;

        public static GameManager GetGameManager() {
            return GameObject.Find("GameManager").GetComponent<GameManager>();
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

    }
}
