using System;
using TrustfallGames.KeepTalkingAndEscape.Manager;
using UnityEngine;
using UnityEngine.UI;
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
        [SerializeField] private bool _unlockObjectWhenNumButtonActive;
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
        [SerializeField] private AudioClip _blockSound;

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

        [SerializeField] private string _damageGhostMessage;
        [SerializeField] private string _damageObjectMessage;
        [SerializeField] private string _damageItemMessage;

        [SerializeField] private GhostParticleHandler _removeParticleOnNoDamage;


        //Determines if its a Radio
        //[SerializeField] private bool _isARadio;

        private bool _damageItemRecieved;
        private bool _damageObjectRecieved;
        private bool _damageGhostRecieved;
        private bool _damageDisabledByGhost;
        private bool _isGameOver;

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
        [SerializeField] private bool _animationObjectMustUnlocked;

        //which item is needed to unlock the object
        [SerializeField] private string _itemToUnlock;
        [SerializeField] private string _lockedFlavourTextGhost;
        [SerializeField] private string _unlockedFlavourTextGhost;
        [SerializeField] private string _lockedFlavourTextHuman;
        [SerializeField] private string _unlockedFlavourTextHuman;

        //All object which must be unlocked to interact with the object.
        [SerializeField] private ObjectInteractionListener[] _objectsToUnlock;

        [SerializeField] private bool _showImageOnInteraction;
        [SerializeField] private Sprite _ghostImage;
        [SerializeField] private Sprite _humanImage;

        //Makes the object to a lightswitch
        [SerializeField] private bool _lightswitch;
        [SerializeField] private GameObject _light;
        [SerializeField] private GameObject _secondLight;
        private bool _objectUnlocked;
        private object _ghostReachableLast;


        private void Start() {
            _uiManager = UIManager.GetUiManager();
            _gameManager = GameManager.GetGameManager();
            _itemManager = ItemManager.GetItemManager();
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
            BlockSoundAttacher();
            UpdateMotherState();
            UpdateDamageParticle();
        }

        /// <summary>
        /// Destroys the particle Component, if all damage is disabled.
        /// </summary>
        private void UpdateDamageParticle() {
            if(_removeParticleOnNoDamage == null) return;
            var damageDisabled = true;
            if(_disableDamageWithObject != null) {
                damageDisabled = _disableDamageWithObject._damageDisabled;
            }

            if(_disableDamageByGhost) {
                if(damageDisabled) {
                    damageDisabled = _damageDisabledByGhost;
                }
            }

            if(damageDisabled) {
                Destroy(_removeParticleOnNoDamage);
                Destroy(GameObject.Find(gameObject.name + "/Particle"));
            }
        }

        private void UpdateHumanMap() {
            if(_mapHandler != null) {
                if(_mapHandler.HumanMapActive != _humanMapActiveLast) {
                    _humanMapActiveLast = _mapHandler.HumanMapActive;
                    _gameManager.HumanMapActive = _mapHandler.HumanMapActive;
                }

                _humanMapActiveLast = _mapHandler.HumanMapActive;
            }
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
            if(_numButtonHandler != null)
                _objectUnlocked = _numButtonHandler.CodeSolved;
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
            if(_gameManager.HumanController.Health <= 0) return;
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

            if(!_ghostReachable && _audioSource.clip == _soundManager.WaterSound && _audioSource.isPlaying) {
                _audioSource.Stop();
                _ghostInactiveObject.SetActive(true);
                _ghostActiveObject.SetActive(false);
            }

            if(!_ghostReachable) return;
            if(_gameManager.GhostDrivenAnimationActive) return;
            if(Input.GetButtonDown(ButtonNames.GhostInspect)) {
                _uiManager.GhostFlavourText = _ghostFlavourText;
                if(_itemToUnlock != "" && _objectUnlocked)
                    _uiManager.GhostFlavourText = _unlockedFlavourTextGhost;
                if(_itemToUnlock != "" && !_objectUnlocked) {
                    _uiManager.GhostFlavourText = _lockedFlavourTextGhost;
                }
                if(_numButtonHandler != null && _objectUnlocked)
                    _uiManager.GhostFlavourText = _unlockedFlavourTextGhost;
                if(_numButtonHandler != null && !_objectUnlocked) {
                    _uiManager.GhostFlavourText = _unlockedFlavourTextGhost;
                }
            }

            if(_activateObjectWithGhostInteraction) {
                if(Input.GetButton(ButtonNames.GhostInteract)) {
                    if(!_audioSource.isPlaying) {
                        _audioSource.clip = _soundManager.WaterSound;
                        _audioSource.Play();
                        _audioSource.loop = true;
                        _ghostActiveObject.SetActive(true);
                        _ghostInactiveObject.SetActive(false);
                    }
                }
                else {
                    if(_audioSource.clip == _soundManager.WaterSound && _audioSource.isPlaying) {
                        _audioSource.Stop();
                        _ghostInactiveObject.SetActive(true);
                        _ghostActiveObject.SetActive(false);
                    }
                }

                return;
            }

            if(Input.GetButtonDown(ButtonNames.GhostInteract)) {
                if(_gameManager.GhostImageCanvasActive || _uiManager.InventoryGhost.InventoryActive) return;
                //Disables damage for linked object
                if(_disableDamageByGhost) _damageDisabledByGhost = true;
                if(_itemIdToUnlock != "" && _animationUnlocked && _showImageOnInteraction) {
                    _uiManager.ShowImage(CharacterType.Ghost, _ghostImage);
                }

                if(_itemIdToUnlock == "" && _showImageOnInteraction) {
                    _uiManager.ShowImage(CharacterType.Ghost, _ghostImage);
                }


                if(AnimationType == AnimationType.None) return;
                if(_animationType == AnimationType.Open) {
                    if(!_ghostCanOpen) {
                        return;
                    }

                    var unlocked = IsObjectUnlocked();

                    if(!unlocked) return;

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
                if(_itemToUnlock != "" && _objectUnlocked)
                    _uiManager.HumanFlavourText = _unlockedFlavourTextHuman;
                if(_itemToUnlock != "" && !_objectUnlocked) {
                    _uiManager.HumanFlavourText = _lockedFlavourTextHuman;
                }
                if(_numButtonHandler != null && _objectUnlocked)
                    _uiManager.HumanFlavourText = _unlockedFlavourTextHuman;
                if(_numButtonHandler != null && !_objectUnlocked) {
                    _uiManager.HumanFlavourText = _lockedFlavourTextHuman;
                }
            }

            if(Input.GetButtonDown(ButtonNames.HumanInteract)) {
                if(_gameManager.HumanImageCanvasActive || _gameManager.HumanMapActive || _uiManager.InventoryHuman.InventoryActive || _gameManager.HumanNumPadActive) return;
                if(_itemToUnlock != "") {
                    if(string.Equals(_uiManager.InventoryHuman.ItemInHand, _itemToUnlock, StringComparison.CurrentCultureIgnoreCase)) {
                        _objectUnlocked = true;
                        _itemManager.RemoveItemFromHandAndInventory();
                    }
                }

                if(_itemIdToUnlock != "" && _animationUnlocked && _showImageOnInteraction) {
                    _uiManager.ShowImage(CharacterType.Human, _humanImage);
                }

                if(_itemIdToUnlock == "" && _showImageOnInteraction) {
                    _uiManager.ShowImage(CharacterType.Human, _humanImage);
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
                else if(_canBeTakenToInventory && _canBePickedUpAfterGhostAction) {
                    _uiManager.HumanFlavourText = _blockedMessage;
                }

                if(_objectActiveToGetItem) {
                    if(!_ghostActiveObject.activeSelf) return;
                }

                // Put gameobject only in inventory but disables further inventory adding
                else if(_canBeTakenButStayInScene && !_itemRequiredToRecieveItem) {
                    _canBeTakenButStayInScene = false;
                    _soundManager.Source.clip = _soundManager.PickupSound;
                    _soundManager.Source.Play();
                    _itemManager.RemoveItemFromHandAndInventory();
                    _itemManager.AddItemToInv(_itemName);

                    //Combine Item and Object in scene to get a new Item
                }

                if(_canBeTakenButStayInScene && _itemRequiredToRecieveItem) {
                    if(string.Equals(_uiManager.InventoryHuman.ItemInHand, _itemNameRequiredToRecieveItem, StringComparison.CurrentCultureIgnoreCase)) {
                        _soundManager.Source.clip = _soundManager.PickupSound;
                        _soundManager.Source.Play();
                        _itemManager.RemoveItemFromHandAndInventory();
                        _itemManager.AddItemToInv(_itemName);
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

                if(_animationType == AnimationType.None
                   && _unlockObjectWhenNumButtonActive
                   && !_numButtonHandler.CodeSolved) {
                    _numButtonHandler.OpenButtonField();
                }

                //Starts Animation, if it isnt disabled
                if(AnimationType == AnimationType.Open) {
                    var unlocked = IsObjectUnlocked();

                    if(!unlocked) return;

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

        private bool IsObjectUnlocked() {
            var unlocked = true;

            if(_objectMustUnlocked) {
                foreach(var obj in _objectsToUnlock) {
                    if(obj.ObjectUnlocked == false) {
                        _uiManager.HumanFlavourText = _blockedMessage;
                        unlocked = false;
                        break;
                    }
                }
            }

            if(_animationObjectMustUnlocked) {
                foreach(var obj in _objectsToUnlock) {
                    if(obj._animationUnlocked == false) {
                        _uiManager.HumanFlavourText = _blockedMessage;
                        unlocked = false;
                        break;
                    }
                }
            }

            if(!unlocked) return unlocked;
            return unlocked;
        }

        private bool CalculateDamage() {
            if(_disableDamageWithObject != null && !_damageObjectRecieved)
                if(!_disableDamageWithObject._damageDisabled) {
                    _gameManager.HumanController.TakeHealth(1);
                    _uiManager.HumanFlavourText = _damageObjectMessage;

                    if(_OneTimeDamage) _damageObjectRecieved = true;
                    if(_cancelPickupOnDamage) return true;
                }

            //Item Damage
            if(_disableDamageWithItem != "" && !_damageItemRecieved)
                if(!string.Equals(_uiManager.InventoryHuman.ItemInHand, _disableDamageWithItem,
                                  StringComparison.CurrentCultureIgnoreCase)) {
                    if(_OneTimeDamage) _damageItemRecieved = true;
                    _gameManager.HumanController.TakeHealth(1);
                    _uiManager.HumanFlavourText = _damageItemMessage;
                    if(_cancelPickupOnDamage) return true;
                }
                else {
                    _itemManager.RemoveItemFromHandAndInventory();
                }

            //Ghost Damage
            if(!_damageDisabledByGhost && _disableDamageByGhost && !_damageGhostRecieved) {
                if(_OneTimeDamage) _damageGhostRecieved = true;
                _gameManager.HumanController.TakeHealth(1);
                _uiManager.HumanFlavourText = _damageGhostMessage;
                if(_cancelPickupOnDamage) return true;
            }

            if(_gameManager.HumanController.Health <= 0) {
                _soundManager.Source.clip = _soundManager.DeathSound;
                _soundManager.Source.Play();
                _isGameOver = true;
            }

            return false;
        }

        private void BlockSoundAttacher() {
            if(_blockSound != null && _uiManager.HumanFlavourText == _blockedMessage) {
                _audioSource.clip = _blockSound;
                _audioSource.Play();
            }
        }

        private void UpdateUi() {
            UpdateGhostUi();
            UpdateHumanUi();
        }

        private void UpdateHumanUi() {
            if(!_humanReachable && _uiManager.HumanHoverText == _objectDescription && _objectDescription != "" && _uiManager.GetLastInstanceId(CharacterType.Human) == GetInstanceID()) {
                _uiManager.HideButtons(CharacterType.Human);
                return;
            }

            if(!_humanReachable) return;

            _uiManager.HumanHoverText = _objectDescription;
            if(_humanFlavourText == "") {
                _uiManager.ShowButtons(CharacterType.Human, KeyType.B, KeyType.none, GetInstanceID());
                return;
            }

            if(_humanFlavourText != "")
                _uiManager.ShowButtons(CharacterType.Human, KeyType.B, KeyType.A, GetInstanceID());
        }

        private void UpdateGhostUi() {
            //if(_gameManager.GhostDrivenAnimationActive && _ghostDrivenAnimationActive != true) return;
            if(_ghostDrivenAnimationActive) {
                _uiManager.ShowButtonsAnimation(CharacterType.Ghost, _keyType, KeyType.A);
                return;
            }

            if(!_ghostReachable && _uiManager.GhostHoverText == _objectDescription && _objectDescription != "" && _uiManager.GetLastInstanceId(CharacterType.Ghost) == GetInstanceID()) {
                _uiManager.HideButtons(CharacterType.Ghost);
                return;
            }

            if(!_ghostReachable) {
                return;
            }

            _uiManager.GhostHoverText = _objectDescription;
            if(_activateObjectWithGhostInteraction && _ghostFlavourText == "") {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.none, GetInstanceID());
                return;
            }

            if(_activateObjectWithGhostInteraction && _ghostFlavourText != "") {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.A, GetInstanceID());
                return;
            }

            if(_disableDamageByGhost && !_damageDisabledByGhost && _ghostFlavourText == "") {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.none, GetInstanceID());
                return;
            }

            if(_disableDamageByGhost && !_damageDisabledByGhost && _ghostFlavourText != "") {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.A, GetInstanceID());
                return;
            }

            if(_disableDamageByGhost && _damageDisabledByGhost && _ghostFlavourText != "") {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.A, KeyType.none, GetInstanceID());
                return;
            }

            if(_animationType == AnimationType.GhostMoveOnKeySmash && _ghostFlavourText != "") {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.A, GetInstanceID());
                return;
            }

            if(_animationType == AnimationType.GhostMoveOnKeySmash && _ghostFlavourText == "") {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.none, GetInstanceID());
                return;
            }

            if(_ghostFlavourText == "" && _ghostCanOpen) {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.none, GetInstanceID());
                return;
            }

            if(!_ghostCanOpen && _ghostFlavourText != "" && _showImageOnInteraction && !_animationUnlocked) {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.none, GetInstanceID());
                return;
            }

            if(!_ghostCanOpen && _ghostFlavourText != "" && _showImageOnInteraction && _animationUnlocked) {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.A, GetInstanceID());
                return;
            }

            if(!_ghostCanOpen && _ghostFlavourText != "") {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.A, KeyType.none, GetInstanceID());
                return;
            }

            if(_ghostCanOpen && _ghostFlavourText != "") {
                _uiManager.ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.A, GetInstanceID());
            }
        }

        private bool ObjectInteractable(CharacterType characterType) {
            if(characterType == CharacterType.Ghost && _ghostCanOpen == false) return false;
            if(_animationType != AnimationType.None) return true;
            if(_itemName != "") return true;
            if(_lightswitch) return true;
            return false;
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

            if(_mapHandler != null) {
                _mapHandler.CloseMap();
                _gameManager.HumanMapActive = false;
            }

            if(_canBeTakenToInventory) {
                UIManager.GetUiManager().HideButtons(CharacterType.Human);
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

        public bool IsGameOver {
            get {return _isGameOver;}
        }
    }
}
