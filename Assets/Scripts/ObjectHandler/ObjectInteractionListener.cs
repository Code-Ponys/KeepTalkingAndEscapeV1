using System;
using TrustfallGames.KeepTalkingAndEscape.Manager;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace TrustfallGames.KeepTalkingAndEscape.Listener {
    /// <summary>
    /// The ultimate Object listener to animate, pickup items, and more.
    /// </summary>
    public class ObjectInteractionListener : MonoBehaviour {
        //The Gameobect which holds the script
        [SerializeField] private GameObject _meshGameObject;

        //The second Gameobject which is associated with this one and should be affected by this gameobject
        [SerializeField] private GameObject _secondGameObject;

        //The hover description of the object
        [SerializeField] private string _objectDescription;

        //The object description for inspect Ghost and Human
        [SerializeField] private string _ghostFlavourText;
        [SerializeField] private string _humanFlavourText;

        //the id of the item which should be recieved on interaction
        [SerializeField] private string _itemName;

        //The animation Type
        [SerializeField] private AnimationType _animationType;

        //Activates nummlock for the object. You have to type in a code
        [SerializeField] private bool _animationAllowWhenNumButtonActive;
        private bool _humanNumPadActiveLast;

        //The num Button Object for the ui
        [SerializeField] private NumButtonHandler _numButtonHandler;

        [SerializeField] private bool _animationAllowWhenMapSolved;

        [SerializeField] private MapHandler _mapHandler;
        private bool _humanMapActiveLast;

        //At which state should the child be activated
        [SerializeField] private ActivateChildWhen _activateChildWhen;

        //Should the object be triggered by a mother object. (Second GameObject)
        [SerializeField] private bool _getActivationFromMother;

        //Switch between to gameobjects if active
        [SerializeField] private bool _toggleActiveGameobjectByMother;
        [SerializeField] private GameObject _activeObject;
        [SerializeField] private GameObject _inactiveObject;


        //Which key hsoould be smashed. Only some action
        [SerializeField] private KeyType _keyType;

        //How long should the animation go. In Frames
        [Range(1, 1000)] [SerializeField] private int _animationDurationInFrames = 60;

        //How many steps (frames) should the animation go with one klick.
        [Range(1, 1000)] [SerializeField] private int _animationStepsPerKlick = 10;

        private AnimationController _animationController;

        //The goal coordinates.
        [SerializeField] private Vector3 _positionAnimated;
        [SerializeField] private Vector3 _rotationAnimated;
        [SerializeField] private Vector3 _scaleAnimated;

        //Sound implementation
        private AudioSource _audioSource;
        [SerializeField] private AudioClip _openSound;
        [SerializeField] private AudioClip _closeSound;
//        [SerializeField] private AudioClip _radioSound;

        private UIManager _uiManager;
        private GameManager _gameManager;
        private ItemManager _itemManager;
        private SoundManager _soundManager;
        private bool _humanMessageActive;
        private bool _ghostMessageActive;
        private bool _humanLookingAtMe;
        private bool _ghostLookingAtMe;
        private bool _humanReachable;
        private bool _ghostReachable;
        private bool _ghostDrivenAnimationActive;

        private bool _motherObjectActive;

        //Can the object only picked up after the ghost interacted with the item.
        [SerializeField] private bool _canBePickedUpAfterGhostAction;

        //Should the item can be taken to the inventory and the object should be removed
        [SerializeField] private bool _canBeTakenToInventory;

        //Should the item stay in scene after the item is picked up
        [SerializeField] private bool _canBeTakenButStayInScene;

        [SerializeField] private bool _itemRequiredToRecieveItem;

        [SerializeField] private string _itemNameRequiredToRecieveItem;

        //should the item only be moveable in one direction
        [SerializeField] private bool _onedirectionAnimation;

        //Should the item fall down after the ghost interacted
        [SerializeField] private bool _activateGravityAtEnd;

        //only a human can interact with this item
        [SerializeField] private bool _ghostCanOpen;

        [SerializeField] private bool _calculateDamageBeforePickup;
        [SerializeField] private bool _calculateDamageBeforeAnimation;

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

        //Determines if its a Radio
        //[SerializeField] private bool _isARadio;

        private bool _damageItemRecieved;
        private bool _damageObjectRecieved;
        private bool _damageGhostRecieved;
        private bool _damageDisabledByGhost;

        //Object damage disable
        [SerializeField] private bool _objectCanBeDisabledToAvoidDamage;
        [SerializeField] private GameObject _enabledObject;
        [SerializeField] private GameObject _disabledObject;
        private bool _damageDisabled;

        [SerializeField] private bool _animationBlockedCauseOfObject;
        [SerializeField] private ObjectInteractionListener _animationBlockObject;

        [SerializeField] private bool _objectCanBeDisabledToUnblockAnimation;
        [SerializeField] private bool _objectMustUnlockedWithItemForAnimation;
        [SerializeField] private string _blockedMessage;
        [SerializeField] private string _itemIdToUnlock;
        [SerializeField] private GameObject _disableAnimationObject;
        [SerializeField] private GameObject _enableAnimationObject;
        private bool _objectDisabled;
        private bool _animationUnlocked;

        [SerializeField] private bool _objectActiveToGetItem;
        [SerializeField] private bool _activateObjectWithGhostInteraction;
        [SerializeField] private GameObject _ghostActiveObject;
        [SerializeField] private GameObject _ghostInactiveObject;
        private bool _ghostObjectActive;


        //Must the Objects in object to unlock be unlocked to interact with the object
        [SerializeField] private bool _objectMustUnlocked;

        //which item is needed to unlock the object
        [SerializeField] private string _itemToUnlock;

        //All object which must be unlocked to interact with the object.
        [SerializeField] private ObjectInteractionListener[] _objectsToUnlock;

        //Makes the object to a lightswitch
        [SerializeField] private bool _lightswitch;
        [SerializeField] private GameObject _light;
        [SerializeField] private GameObject _secondLight;
        private bool _objectUnlocked;
        private object _ghostReachableLast;


        private void Start() {
            _uiManager = UIManager.GetUiManager();
            _gameManager = GameManager.GetGameManager();
            _itemManager = ItemManager.GetItemHandler();
            _soundManager = SoundManager.GetSoundManager();
            if(_meshGameObject == null) _meshGameObject = gameObject;

            if(AnimationType != AnimationType.None) _animationController = gameObject.AddComponent<AnimationController>();

            _itemManager.CheckItem(this);

            if(_activeObject != null) _inactiveObject.SetActive(false);
            if(_enableAnimationObject != null) _enableAnimationObject.SetActive(false);
            if(_enabledObject != null) _enabledObject.SetActive(false);
            if(_ghostActiveObject != null) _ghostActiveObject.SetActive(false);

            _audioSource = _meshGameObject.AddComponent<AudioSource>();
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
            UpdateHumanNumPad();
            UpdateHumanMap();
            KeyInteraction();
            UpdateMotherState();
        }

        private void UpdateHumanMap() {
            if(_mapHandler == null) return;
            if(_mapHandler.HumanMapActive != _humanMapActiveLast) {
                _humanMapActiveLast = _mapHandler.HumanMapActive;
                _gameManager.HumanMapActive = _mapHandler.HumanMapActive;
            }

            _humanMapActiveLast = _mapHandler.HumanMapActive;
        }


        private void UpdateMotherState() {
            if(_getActivationFromMother) {
                if(_secondGameObject.GetComponent<AnimationController>() == null) throw new Exception(gameObject + "has no Animation Controller");
                _motherObjectActive = _secondGameObject.GetComponent<AnimationController>().Open;
            }
            else
                return;

            if(_toggleActiveGameobjectByMother)
                if(_motherObjectActive) {
                    _activeObject.SetActive(true);
                    _inactiveObject.SetActive(false);
                }
                else {
                    _activeObject.SetActive(false);
                    _inactiveObject.SetActive(true);
                }
        }

        private void UpdateGhostDrivenAnimation() {
            if(_animationController == null) return;
            if(_animationController.GhostDrivenAnimationActive != _ghostDrivenAnimationActive) {
                _ghostDrivenAnimationActive = _animationController.GhostDrivenAnimationActive;
                _gameManager.GhostDrivenAnimationActive = _animationController.GhostDrivenAnimationActive;
            }

            _ghostDrivenAnimationActive = _animationController.GhostDrivenAnimationActive;
        }

        private void UpdateHumanNumPad() {
            if(_numButtonHandler == null) return;
            if(_numButtonHandler.HumanNumPadActive != _humanNumPadActiveLast) {
                _humanNumPadActiveLast = _numButtonHandler.HumanNumPadActive;
                _gameManager.HumanNumPadActive = _numButtonHandler.HumanNumPadActive;
            }

            _humanNumPadActiveLast = _numButtonHandler.HumanNumPadActive;
        }

        /// <summary>
        ///     Checks which player is looking at the object. Raycast shows how far the player is from the object
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
                _humanLookingAtMe = obj == _meshGameObject;
            }
        }

        /// <summary>
        ///     Controls the Inputs made by the players
        /// </summary>
        private void KeyInteraction() {
            KeyInteractionHuman();

            //Mostly same like player 1 but for player 2
            KeyInteractionGhost();
        }

        /// <summary>
        ///     KeyInteraction for Ghost aka Player 2
        /// </summary>
        private void KeyInteractionGhost() {
            if(!_ghostReachable && _ghostActiveObject != null && _ghostActiveObject.activeSelf) {
                _ghostInactiveObject.SetActive(true);
                _ghostActiveObject.SetActive(false);
            }

            if(!_ghostReachable) return;
            if(_gameManager.GhostDrivenAnimationActive) return;
            if(Input.GetButtonDown(ButtonNames.GhostInspect)) {
                _uiManager.GhostFlavourText = _ghostFlavourText;
            }

            if(_activateObjectWithGhostInteraction) {
                _ghostInactiveObject.SetActive(!Input.GetButton(ButtonNames.GhostInteract));
                _ghostActiveObject.SetActive(Input.GetButton(ButtonNames.GhostInteract));
                return;
            }

            if(Input.GetButtonDown(ButtonNames.GhostInteract)) {
                //Disables damage for linked object
                if(_disableDamageByGhost) _damageDisabledByGhost = true;


                if(AnimationType == AnimationType.None) return;
                if(_animationType == AnimationType.Open) {
                    if(!_ghostCanOpen) return;
                    if(_objectMustUnlocked) {
                        foreach(var obj in _objectsToUnlock) {
                            if(obj.ObjectUnlocked == false)
                                _uiManager.GhostFlavourText = _blockedMessage;
                            return;
                        }
                    }

                    if(_animationAllowWhenNumButtonActive && !_numButtonHandler.CodeSolved) return;
                    if(_animationAllowWhenMapSolved && !_mapHandler.CodeSolved) return;


                    _animationController.StartNewAnimation(this);
                }

                switch(_animationType) {
                    case AnimationType.GhostMoveOnKeySmash:
                        _animationController.StartNewAnimation(this);
                        break;
                    case AnimationType.GhostActivateOnKeyHold:
                        _animationController.StartNewAnimation(this);
                        break;
                }
            }
        }

        /// <summary>
        ///     KeyInteraction for Human aka Player 1
        /// </summary>
        /// <returns></returns>
        private void KeyInteractionHuman() {
            if(!_humanReachable) return;

            if(Input.GetButtonDown(ButtonNames.HumanInspect)) {
                _uiManager.HumanFlavourText = _humanFlavourText;
            }

            if(Input.GetButtonDown(ButtonNames.HumanInteract)) {
                if(_itemToUnlock != "") {
                    if(string.Equals(_uiManager.InventoryHuman.ItemInHand, _itemToUnlock, StringComparison.CurrentCultureIgnoreCase)) {
                        _objectUnlocked = true;
                        _itemManager.RemoveItemFromHandAndInventory();
                    }
                }

                if(_lightswitch) {
                    _light.SetActive(!_light.activeSelf);
                    if(_secondLight != null) {
                        _secondLight.SetActive(!_secondLight.activeSelf);
                    }
                }

                if(_objectMustUnlockedWithItemForAnimation && !_animationUnlocked) {
                    if(string.Equals(_uiManager.InventoryHuman.ItemInHand, _itemIdToUnlock,
                                     StringComparison.CurrentCultureIgnoreCase)) {
                        _animationUnlocked = true;
                        _itemManager.RemoveItemFromHandAndInventory();
                        _disableAnimationObject.SetActive(false);
                        _enableAnimationObject.SetActive(true);
                    }
                    else {
                        _uiManager.HumanFlavourText = _blockedMessage;
                        return;
                    }
                }


                //Object Damage
                if(_calculateDamageBeforePickup)
                    if(CalculateDamage())
                        return;

                // Disable GameObject and Put the Gameobject in the Inventory
                if(_canBeTakenToInventory && !_canBePickedUpAfterGhostAction) {
                    _soundManager.Source.clip = _soundManager.PickupSound;
                    _soundManager.Source.Play();
                    _itemManager.AddItemToInv(_itemName);
                    _uiManager.HumanHoverText = "";
                    _meshGameObject.SetActive(false);
                }

                if(_objectActiveToGetItem) {
                    if(!_ghostActiveObject.activeSelf) return;
                }

                // Put gameobject only in inventory but disables further inventory adding
                else if(_canBeTakenButStayInScene && !_itemRequiredToRecieveItem) {
                    _canBeTakenButStayInScene = false;
                    _itemManager.AddItemToInv(_itemName);

                    //Combine Item and Object in scene to get a new Item
                }

                if(_canBeTakenButStayInScene && _itemRequiredToRecieveItem) {
                    if(string.Equals(_uiManager.InventoryHuman.ItemInHand, _itemNameRequiredToRecieveItem, StringComparison.CurrentCultureIgnoreCase)) {
                        _itemManager.AddItemToInv(_itemName);
                        _itemManager.RemoveItemFromHandAndInventory();
                        _canBeTakenButStayInScene = false;
                    }
                }


                if(_objectCanBeDisabledToAvoidDamage) {
                    if(_damageDisabled) return;
                    _enabledObject.SetActive(false);
                    _disabledObject.SetActive(true);
                    _damageDisabled = true;
                }

                if(_objectCanBeDisabledToUnblockAnimation) {
                    if(_objectDisabled) return;
                    _objectDisabled = true;
                    _disableAnimationObject.SetActive(false);
                    _enableAnimationObject.SetActive(true);
                }

                if(_animationBlockedCauseOfObject) {
                    if(!_animationBlockObject._objectDisabled) {
                        _uiManager.HumanFlavourText = _blockedMessage;
                        return;
                    }
                }

                //Starts Animation, if it isnt disabled
                if(AnimationType == AnimationType.Open) {
                    if(_objectMustUnlocked)
                        foreach(var obj in _objectsToUnlock) {
                            if(obj.ObjectUnlocked == false) {
                                _uiManager.HumanFlavourText = _blockedMessage;
                                return;
                            }
                        }

                    if(_animationAllowWhenNumButtonActive && !_numButtonHandler.CodeSolved) {
                        _numButtonHandler.OpenButtonField();
                        return;
                    }

                    if(_animationAllowWhenMapSolved && !_mapHandler.CodeSolved) {
                        _mapHandler.OpenMap();
                        return;
                    }

                    if(_calculateDamageBeforeAnimation)
                        CalculateDamage();

                    _animationController.StartNewAnimation(this);
                }

                //Used for Linked objects
                if(_animationType == AnimationType.OpenLinkedOnHold) _secondGameObject.GetComponent<ObjectInteractionListener>().StartAnimation(_meshGameObject);
            }
        }

        private bool CalculateDamage() {
            if(_disableDamageWithObject != null && !_damageObjectRecieved)
                if(!_disableDamageWithObject._damageDisabled) {
                    _gameManager.HumanController.TakeHealth(1);
                    if(_OneTimeDamage) _damageObjectRecieved = true;
                    if(_cancelPickupOnDamage) return true;
                }

            //Item Damage
            if(_disableDamageWithItem != "" && !_damageItemRecieved)
                if(!string.Equals(_uiManager.InventoryHuman.ItemInHand, _disableDamageWithItem,
                                  StringComparison.CurrentCultureIgnoreCase)) {
                    if(_OneTimeDamage) _damageItemRecieved = true;
                    _gameManager.HumanController.TakeHealth(1);
                    if(_cancelPickupOnDamage) return true;
                }
                else {
                    _itemManager.RemoveItemFromHandAndInventory();
                }

            //Ghost Damage
            if(!_damageDisabledByGhost && _disableDamageByGhost && !_damageGhostRecieved) {
                Debug.Log("Ghost Damage Recieved");
                if(_OneTimeDamage) _damageGhostRecieved = true;
                _gameManager.HumanController.TakeHealth(1);
                if(_cancelPickupOnDamage) return true;
            }

            return false;
        }

        private void UpdateUi() {
            UpdateGhostUi();
            UpdateHumanUi();
        }

        private void UpdateHumanUi() {
            if(!_humanReachable && _uiManager.HumanHoverText == _objectDescription && _objectDescription != "") {
                _uiManager.HumanHoverText = "";
                Debug.Log(gameObject.name + "entfernt buttons");

                _uiManager.HideButtons(CharacterType.Human);
                return;
            }

            if(!_humanReachable) return;

            _uiManager.HumanHoverText = _objectDescription;
            if(_humanFlavourText == "") {
                _uiManager.ShowButtons(CharacterType.Human, KeyType.B, KeyType.none);
            }
            else {
                _uiManager.ShowButtons(CharacterType.Human, KeyType.B, KeyType.A);
            }
        }

        private void UpdateGhostUi() {
            //if(_gameManager.GhostDrivenAnimationActive && _ghostDrivenAnimationActive != true) return;
            if(_ghostDrivenAnimationActive) {
                _uiManager.ShowButtonsAnimation(CharacterType.Ghost, _keyType, KeyType.A);
                return;
            }

            if(!_ghostReachable && _uiManager.GhostHoverText == _objectDescription && _objectDescription != "") {
                _uiManager.GhostHoverText = "";
                Debug.Log(gameObject.name + "entfernt buttons");
                _uiManager.HideButtons(CharacterType.Ghost);
                return;
            }

            if(!_ghostReachable) {
                return;
            }

            _uiManager.GhostHoverText = _objectDescription;
            if(_activateObjectWithGhostInteraction && _ghostFlavourText == "") {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.none);
                return;
            }

            if(_activateObjectWithGhostInteraction && _ghostFlavourText != "") {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.A);
                return;
            }

            if(_disableDamageByGhost && !_damageDisabledByGhost && _ghostFlavourText == "") {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.none);
                return;
            }

            if(_disableDamageByGhost && !_damageDisabledByGhost && _ghostFlavourText != "") {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.A);
                return;
            }

            if(_disableDamageByGhost && _damageDisabledByGhost && _ghostFlavourText != "") {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.A, KeyType.none);
                return;
            }

            if(_animationType == AnimationType.GhostMoveOnKeySmash && _ghostFlavourText != "") {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.A);
                return;
            }

            if(_animationType == AnimationType.GhostMoveOnKeySmash && _ghostFlavourText == "") {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.none);
                return;
            }

            if(_ghostFlavourText == "" && _ghostCanOpen) {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.none);
                return;
            }

            if(!_ghostCanOpen && _ghostFlavourText != "") {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.A, KeyType.none);
                return;
            }

            if(_ghostCanOpen && _ghostFlavourText != "") {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.A);
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
                if(closesPointToCharacter.y > _gameManager.GhostHeight) return false;
            }
            else {
                if(closesPointToCharacter.y > _gameManager.HumanHeight) return false;
            }

            closesPointToCharacter.y = 0;
            var distance = Vector3.Distance(characterpos, closesPointToCharacter);
            return distance < 150;
        }

        private void StartAnimation(GameObject parentGameObject) {
            _animationController.StartNewAnimation(parentGameObject, this);
        }

        public bool IsHumanPressingAgainsObject() {
            if(!_humanReachable) return false;
            return _animationType == AnimationType.OpenLinkedOnHold && Input.GetButton(ButtonNames.HumanInteract);
        }

        private void OnDisable() {
            if(_numButtonHandler != null) {
                _numButtonHandler.CloseButtonField();
                _gameManager.HumanNumPadActive = false;
            }
        }

        public int AnimationDurationInFrames {
            get {return _animationDurationInFrames;}
            set {_animationDurationInFrames = value;}
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

        public bool CanBePickedUpAfterGhostAction {
            get {return _canBePickedUpAfterGhostAction;}
            set {_canBePickedUpAfterGhostAction = value;}
        }

        public bool CanBeTakenToInventory {
            get {return _canBeTakenToInventory;}
            set {_canBeTakenToInventory = value;}
        }

        public bool CanBeTakenButStayInScene {
            get {return _canBeTakenButStayInScene;}
            set {_canBeTakenButStayInScene = value;}
        }

        public string ItemName {
            get {return _itemName;}
        }

        public AudioClip OpenSound {
            get {return _openSound;}
        }

        public AudioClip CloseSound {
            get {return _closeSound;}
        }

//        public AudioClip RadioSound {
//            get {return _radioSound;}
//        }

//        public bool IsARadio {
//            get {return _isARadio;}
//        }

        public GameObject MeshGameObject {
            get {return _meshGameObject;}
        }

        public AudioSource Source {
            get {return _audioSource;}
            set {_audioSource = value;}
        }
    }
}
