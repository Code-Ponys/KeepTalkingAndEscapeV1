using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography.X509Certificates;
using System.Timers;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using TrustfallGames.KeepTalkingAndEscape.Manager;
using UnityEditor;

namespace TrustfallGames.KeepTalkingAndEscape.Listener {
    public class ObjectInteractionListener : MonoBehaviour {
        //The Gameobect which holds the script
        private GameObject _meshGameObject;
        //The second Gameobject which is associated with this one and should be affected by this gameobject
        [SerializeField] private GameObject _secondGameObject;
        //The hover description of the object
        [SerializeField] private string _objectDescription;
        //The object description for inspect
        [SerializeField] private string _objectFlavourText;
        //the id of the item which should be recieved on interaction
        [SerializeField] private string _itemName;

        //The animation Type
        [SerializeField] private AnimationType _animationType;
        //Activates nummlock for the object. You have to type in a code
        [SerializeField] private bool _animationAllowWhenNumButtonActive;
        //The num Button Object for the ui
        [SerializeField] private NumButtonHandler _numButtonHandler;
        //At which state should the child be activated
        [SerializeField] private ActivateChildWhen _activateChildWhen;
        //Should the animation be triggered by a mother object. (Second GameObject)
        [SerializeField] private bool _getActivationFromMother;

        //Which key hsoould be smashed. Only some action
        [SerializeField] private KeyType _keyType;
        //How long should the animation go. In Frames
        [UnityEngine.Range(1, 1000)] [SerializeField] private int _animationDurationInFrames = 60;
        //How many steps (frames) should the animation go with one klick.
        [UnityEngine.Range(1, 1000)] [SerializeField] private int _animationStepsPerKlick = 10;

        private AnimationController _animationController;
        private Vector3 _positionBase;
        private Vector3 _rotationBase;
        private Vector3 _scaleBase;

        //The goal coordinates.
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
        //Can the object only picked up after the ghost interacted with the item.
        [SerializeField] private bool _canBePickedUpAfterGhostAction;
        //Should the item can be taken to the inventory and the object should be removed
        [SerializeField] private bool _canBeTakenToInventory;
        //Should the item stay in scene after the item is picked up
        [SerializeField] private bool _canBeTakenButStayInScene;
        //should the item only be moveable in one direction
        [SerializeField] private bool _onedirectionAnimation = false;
        //Should the item fall down after the ghost interacted
        [SerializeField] private bool _activateGravityAtEnd;
        //only a human can interact with this item
        [SerializeField] private bool _onlyHuman;
        //The item id which disables damage.
        [SerializeField] private string _disableDamageWithItem;
        //The object which can be disabled
        [SerializeField] private ObjectInteractionListener _disableDamageWithObject;
        //Should item pickup be canceled on damage
        [SerializeField] private bool _cancelPickupOnDamage;
        //Each damage type can recieved only one time. So max 3 HP can be taken by one object
        [SerializeField] private bool _OneTimeDamage;
        //Damage can be disabled by Ghost
        [SerializeField] private bool _disableDamageByGhost;

        private bool _damageDisabled;
        private bool _damageItemRecieved;
        private bool _damageObjectRecieved;
        private bool _damageGhostRecieved;
        private bool _damageDisabledByGhost;

        //Must the Objects in object to unlock be unlocked to interact with the object
        [SerializeField] private bool _objectMustUnlocked;
        private bool _objectUnlocked;
        //which item is needed to unlock the object
        [SerializeField] private string _itemToUnlock;

        //All object which must be unlocked to interact with the object.
        [SerializeField] private ObjectInteractionListener[] _objectsToUnlock;


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
            var ghostCamera = _gameManager.GhostCamera;
            var humanCamera = _gameManager.HumanCamera;


            RaycastHit hit;
            //Check Ghost;
            var cameraCenter = ghostCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f,
                                                                          ghostCamera.nearClipPlane));
            if(Physics.Raycast(cameraCenter, ghostCamera.transform.forward, out hit, 300)) {
                var obj = hit.transform.gameObject;
                _ghostLookingAtMe = obj == _meshGameObject;
            }

            //Check Human;
            cameraCenter =
                humanCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f,
                                                           humanCamera.nearClipPlane));
            if(Physics.Raycast(cameraCenter, humanCamera.transform.forward, out hit, 300)) {
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
            KeyInteractionHuman();

            //Mostly same like player 1 but for player 2
            KeyInteractionGhost();
        }

        /// <summary>
        /// KeyInteraction for Ghost aka Player 2
        /// </summary>
        private void KeyInteractionGhost() {
            if(_ghostReachable && !_onlyHuman) {
                if(_gameManager.GhostDrivenAnimationActive) return;
                if(Input.GetButtonDown(ButtonNames.GhostInspect)) {
                    _uiManager.GhostFlavourText = _objectFlavourText;
                }
                else if(Input.GetButtonDown(ButtonNames.GhostInteract)) {
                    //Disables damage for linked object
                    if(_disableDamageByGhost) {
                        _damageDisabledByGhost = true;
                    }

                    if(AnimationType != AnimationType.None) {
                        if(_animationType == AnimationType.Open)
                            if(_objectMustUnlocked) {
                                foreach(var obj in _objectsToUnlock) {
                                    if(obj._objectUnlocked == false)
                                        _uiManager.GhostFlavourText = "Blockiert";
                                    return;
                                }
                            }

                        if(_animationAllowWhenNumButtonActive && !_numButtonHandler.CodeSolved) {
                            return;
                        }

                        _animationController.StartNewAnimation(this);
                        if(_animationType == AnimationType.GhostMoveOnKeySmash)
                            _animationController.StartNewAnimation(this);
                        if(_animationType == AnimationType.GhostActivateOnKeyHold)
                            _animationController.StartNewAnimation(this);
                    }
                }
            }
        }

        /// <summary>
        /// KeyInteraction for Human aka Player 1
        /// </summary>
        /// <returns></returns>
        private void KeyInteractionHuman() {
            if(!_humanReachable) return;

            if(Input.GetButtonDown(ButtonNames.HumanInspect)) {
                _uiManager.HumanFlavourText = _objectFlavourText;
            }

            else if(Input.GetButtonDown(ButtonNames.HumanInteract)) {
                //Object Damage
                if(_disableDamageWithObject != null && !_damageObjectRecieved) {
                    if(!_disableDamageWithObject._damageDisabled) {
                        _gameManager.Human.GetComponent<FirstPersonControllerHuman>().TakeHealth(1);
                        if(_OneTimeDamage) _damageObjectRecieved = true;
                        if(_cancelPickupOnDamage) return;
                    }
                }

                //Item Damage
                if(_disableDamageWithItem != "" && !_damageItemRecieved) {
                    if(!string.Equals(Inventory.GetInstance(CharacterType.Human).ItemInHand, _disableDamageWithItem,
                                      StringComparison.CurrentCultureIgnoreCase)) {
                        _gameManager.Human.GetComponent<FirstPersonControllerHuman>().TakeHealth(1);
                        if(_OneTimeDamage) _damageItemRecieved = true;
                        if(_cancelPickupOnDamage) return;
                    }
                }

                //Ghost Damage
                if(!_damageDisabledByGhost && _disableDamageByGhost && !_damageGhostRecieved) {
                    if(_OneTimeDamage) _damageGhostRecieved = true;
                    if(_cancelPickupOnDamage) return;
                }

                // Disable GameObject and Put the Gameobject in the Inventory
                if(_canBeTakenToInventory && _canBePickedUpAfterGhostAction != true) {
                    _itemHandler.AddItemToInv(_itemName);
                    _uiManager.HumanHoverText = "";
                    _meshGameObject.SetActive(false);
                }
                // Put gameobject only in inventory but disables further inventory adding
                else if(_canBeTakenButStayInScene) {
                    _canBeTakenButStayInScene = false;
                    _itemHandler.AddItemToInv(_itemName);
                }


                //Starts Animation, if it isnt disabled
                if(AnimationType == AnimationType.Open) {
                    if(_objectMustUnlocked) {
                        foreach(var obj in _objectsToUnlock) {
                            if(obj._objectUnlocked == false)
                                _uiManager.HumanFlavourText = "Blockiert";
                            return;
                        }
                    }

                    if(_animationAllowWhenNumButtonActive && !_numButtonHandler.CodeSolved) {
                        _numButtonHandler.OpenButtonField();
                        return;
                    }

                    _animationController.StartNewAnimation(this);
                }

                //Used for Linked objects
                if(_animationType == AnimationType.OpenLinkedOnHold) {
                    _secondGameObject.GetComponent<ObjectInteractionListener>().StartAnimation(_meshGameObject);
                }

                if(_itemToUnlock == "") return;
                if(string.Equals(Inventory.GetInstance(CharacterType.Human).ItemInHand, _itemToUnlock, StringComparison.CurrentCultureIgnoreCase)) {
                    _objectUnlocked = true;
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
            if(obj == _gameManager.Ghost) {
                if(closesPointToCharacter.y > 450) {
                    return false;
                }
            }
            else {
                if(closesPointToCharacter.y > 350) {
                    return false;
                }
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

        public bool ObjectUnlocked {
            get {return _objectUnlocked;}
        }
    }
}