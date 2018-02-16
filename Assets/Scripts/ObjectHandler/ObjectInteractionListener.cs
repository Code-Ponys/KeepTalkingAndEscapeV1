using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography.X509Certificates;
using System.Timers;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using TrustfallGames.KeepTalkingAndEscape.Manager;

namespace TrustfallGames.KeepTalkingAndEscape.Listener {
    public class ObjectInteractionListener : MonoBehaviour {
        [SerializeField] private GameObject _meshGameObject;
        [SerializeField] private GameObject _SecondGameObject;
        [SerializeField] private string _objectDescription;
        [SerializeField] private string _objectFlavourText;
        [SerializeField] private string _itemName;

        [SerializeField] private AnimationType _animationType;
        [SerializeField] private ActivateChildWhen _activateChildWhen;
        [SerializeField] private bool _getActivationFromMother;
                               
        [SerializeField] private float _flavourTextWaitTimer = 5f;
        private float _timer = 0f;

        [SerializeField] private KeyType _keyType;
        [Range(1, 1000)] [SerializeField] private int _animationDurationInFrames = 60;
        [Range(1, 1000)] [SerializeField] private int _animationStepsPerKlick = 10;

        private AnimationController _animationController;
        private Vector3 _positionBase;
        private Vector3 _rotationBase;
        private Vector3 _scaleBase;

        [SerializeField] private Vector3 _positionAnimated;
        [SerializeField] private Vector3 _rotationAnimated;
        [SerializeField] private Vector3 _scaleAnimated;

        private UIManager _uiManager;
        private GameManager _gameManager;
        private ItemHandler _itemHandler;
        private bool _humanMessageActive;
        private bool _ghostMessageActive;
        private bool _humanLookingAtMe;
        private bool _ghostLookingAtMe;
        private bool _humanReachable;
        private bool _ghostReachable;
        private bool _ghostDrivenAnimationActive;
        private bool _ghostDrivenAnimationActiveLast;
        private bool _motherObjectActive;
        [SerializeField] public bool _canBePickedUpAfterGhostAction;
        [SerializeField] public bool _canBeTakenToInventory;
        [SerializeField] private bool _canBeTakenButStayInScene;
        [SerializeField] private bool _onedirectionAnimation = false;
        [SerializeField] private bool _activateGravityAtEnd;
        [SerializeField] private bool _onlyHuman;
        [SerializeField] private bool _canObjectDamage;
        [SerializeField] private bool _canDisableObjectDamage;


        private void Start() {
            _uiManager = UIManager.GetUiManager();
            _gameManager = GameManager.GetGameManager();
            _itemHandler = ItemHandler.GetItemHandler();
            
            _uiManager.GhostHoverText = "";
            _uiManager.GhostFlavourText = "";
            _uiManager.HumanHoverText = "";
            _uiManager.HumanFlavourText = "";
            
            if(AnimationType != AnimationType.None) {
                _animationController = _meshGameObject.AddComponent<AnimationController>();
            }

            _positionBase = _meshGameObject.transform.localPosition;
            _rotationBase = _meshGameObject.transform.localRotation.eulerAngles;
            _scaleBase = _meshGameObject.transform.localScale;
        }

        private void Update() {
            IsCharacterLookingAtMe();
            if(_ghostLookingAtMe) {
                _ghostReachable = CanCharacterReach(_gameManager.Ghost);
            }
            else {
                _ghostReachable = false;
                _humanReachable = false;
            }

            if(_humanLookingAtMe) {
                _humanReachable = CanCharacterReach(_gameManager.Human);
            }
            else {
                _humanLookingAtMe = false;
                _humanReachable = false;
            }

            UpdateUi();

            UpdateGhostDrivenAnimation();
            KeyInteraction();
            UpdateMotherState();
        }
        
        
        private void UpdateMotherState() {
            if(_getActivationFromMother) {
                if(_SecondGameObject.GetComponent<AnimationController>().Open) {
                    _motherObjectActive = true;
                }
                else {
                    _motherObjectActive = false;
                }
            }
        }

        private void UpdateGhostDrivenAnimation() {
            if(_animationController == null) return;
            if(_animationController.GhostDrivenAnimationActive != _ghostDrivenAnimationActiveLast) {
                _ghostDrivenAnimationActiveLast = _animationController.GhostDrivenAnimationActive;
                _gameManager.GhostDrivenAnimationActive = _animationController.GhostDrivenAnimationActive;
            }

            _ghostDrivenAnimationActiveLast = _animationController.GhostDrivenAnimationActive;
        }
        
        /// <summary>
        /// Checks which player is looking at the object. Raycast shows how far the player is from the object
        /// </summary>
        private void IsCharacterLookingAtMe() {
            Camera _ghostCamera = _gameManager.GhostCamera;
            Camera _humanCamera = _gameManager.HumanCamera;


            RaycastHit hit;
            //Check Ghost;
            var cameraCenter = _ghostCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f,
                _ghostCamera.nearClipPlane));
            if(Physics.Raycast(cameraCenter, _ghostCamera.transform.forward, out hit, 300)) {
                var obj = hit.transform.gameObject;
                if(obj == _meshGameObject) {
                    _ghostLookingAtMe = true;
                }
                else {
                    _ghostLookingAtMe = false;
                }
            }

            //Check Human;
            cameraCenter =
                _humanCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f,
                    _humanCamera.nearClipPlane));
            if(Physics.Raycast(cameraCenter, _humanCamera.transform.forward, out hit, 300)) {
                var obj = hit.transform.gameObject;
                if(obj == _meshGameObject) {
                    _humanLookingAtMe = true;
                }
                else {
                    _humanLookingAtMe = false;
                }
            }
        }
        
        /// <summary>
        /// Controls the Inputs made by the players
        /// </summary>
        private void KeyInteraction() {
            if(_humanReachable) {
                if(Input.GetButtonDown(ButtonNames.HumanInspect)) {
                    _uiManager.HumanFlavourText = _objectFlavourText;
                }
                else if(Input.GetButtonDown(ButtonNames.HumanInteract)) {
                    if(_canObjectDamage) {
                        _gameManager.Human.GetComponent<FirstPersonControllerHuman>().TakeHealth(1);
                        _uiManager.HealthText = _gameManager.Human.GetComponent<FirstPersonControllerHuman>().Health.ToString();
                    }
                    else {
                        // Disable GameObject and Put the Gameobject in the Inventory
                        if(_canBeTakenToInventory && _canBePickedUpAfterGhostAction != true) {
                            _itemHandler.AddItemToInv(_itemName);
                            _meshGameObject.SetActive(false);
                            _uiManager.HumanHoverText = "";
                        }
                        // Put gameobject only in inventory but disables further inventory adding
                        else if(_canBeTakenButStayInScene) {
                            _canBeTakenButStayInScene = false;
                            _itemHandler.AddItemToInv(_itemName);
                        }
                        //Starts Animation, if it isnt disabled
                        if(AnimationType == AnimationType.Open) {
                            _animationController.StartNewAnimation(this);
                        }
                        //Used for Linked objects
                        if(Input.GetButton(ButtonNames.HumanInteract)) {
                            if(_animationType == AnimationType.OpenLinkedOnHold) {
                                _SecondGameObject.GetComponent<ObjectInteractionListener>().StartAnimation(_meshGameObject);
                            }
                        }
                    }
                }
            }
            //Mostly same like player 1 but for player 2
            if(_ghostReachable && !_onlyHuman) {
                if(_gameManager.GhostDrivenAnimationActive) return;
                if(Input.GetButtonDown(ButtonNames.GhostInspect)) {
                    _uiManager.GhostFlavourText = _objectFlavourText;
                }
                else if(Input.GetButtonDown(ButtonNames.GhostInteract)) {
                    //Disables damage for linked object
                    if(_canDisableObjectDamage) {
                        _SecondGameObject.GetComponent<ObjectInteractionListener>()._canObjectDamage = false;
                    }
                    if(AnimationType != AnimationType.None) {
                        if(_animationType == AnimationType.Open)
                            _animationController.StartNewAnimation(this);
                        if(_animationType == AnimationType.GhostMoveOnKeySmash)
                            _animationController.StartNewAnimation(this);
                        if(_animationType == AnimationType.GhostActivateOnKeyHold)
                            _animationController.StartNewAnimation(this);
                    }
                }
            }
        }

        private void UpdateUi() {
            UpdateGhostUi();
            UpdateHumanUi();
        }

        private void UpdateHumanUi() {
            if(_humanReachable == true && _humanMessageActive == true) return;
            if(_humanReachable && !_humanMessageActive) {
                _uiManager.HumanHoverText = _objectDescription;
                _humanMessageActive = true;
            }

            if(!_humanReachable && _humanMessageActive) {
                _uiManager.HumanHoverText = "";
                _humanMessageActive = false;
            }
        }

        private void UpdateGhostUi() {
            if(_ghostReachable == _ghostMessageActive) return;
            if(_ghostReachable && !_ghostMessageActive) {
                _uiManager.GhostHoverText = _objectDescription;
                _ghostMessageActive = true;
            }

            if(!_ghostReachable && _ghostMessageActive) {
                _uiManager.GhostHoverText = "";
                _ghostMessageActive = false;
            }
        }

        /// <summary>
        /// Determines if the players are close enough to the object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CanCharacterReach(CharacterController obj) {
            var characterpos = obj.GetComponent<Transform>().position;
            var closesPointToCharacter = _meshGameObject.GetComponent<Collider>().ClosestPointOnBounds(characterpos);
            characterpos.y = 0;
            if(closesPointToCharacter.y > 350) {
                return false;
            }
            closesPointToCharacter.y = 0;
            var distance = Vector3.Distance(characterpos, closesPointToCharacter);
            return distance < 150;
        }

        private void StartAnimation(GameObject parentGameObject) {
            _animationController.StartNewAnimation(parentGameObject, this);
        }

        public bool IsHumanPressingAgainsObject() {
            if(_humanReachable) {
                if(_animationType == AnimationType.OpenLinkedOnHold)
                    return Input.GetButton(ButtonNames.HumanInteract);
            }

            return false;
        }

        public int AnimationDurationInFrames {
            get {return _animationDurationInFrames;}
            set {_animationDurationInFrames = value;}
        }

        public Vector3 PositionBase {
            get {return _positionBase;}
            set {_positionBase = value;}
        }

        public Vector3 RotationBase {
            get {return _rotationBase;}
            set {_rotationBase = value;}
        }

        public Vector3 ScaleBase {
            get {return _scaleBase;}
            set {_scaleBase = value;}
        }

        public Vector3 PositionAnimated {
            get {return _positionAnimated;}
            set {_positionAnimated = value;}
        }

        public Vector3 RotationAnimated {
            get {return _rotationAnimated;}
            set {_rotationAnimated = value;}
        }

        public Vector3 ScaleAnimated {
            get {return _scaleAnimated;}
            set {_scaleAnimated = value;}
        }

        public AnimationType AnimationType {
            get {return _animationType;}
            set {_animationType = value;}
        }

        public int AnimationStepsPerKlick {
            get {return _animationStepsPerKlick;}
            set {_animationStepsPerKlick = value;}
        }

        public KeyType KeyType {
            get {return _keyType;}
            set {_keyType = value;}
        }

        public bool OnedirectionAnimation {
            get {return _onedirectionAnimation;}
            set {_onedirectionAnimation = value;}
        }

        public bool ActivateGravityAtEnd {
            get {return _activateGravityAtEnd;}
            set {_activateGravityAtEnd = value;}
        }

        public ActivateChildWhen ActivateChildWhen {
            get {return _activateChildWhen;}
            set {_activateChildWhen = value;}
        }
    }
}