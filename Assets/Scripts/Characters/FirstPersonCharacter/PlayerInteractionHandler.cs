using System;
using System.Collections;
using System.Collections.Generic;
using TrustfallGames.KeepTalkingAndEscape.Listener;
using TrustfallGames.KeepTalkingAndEscape.Manager;
using UnityEngine;

public class PlayerInteractionHandler : MonoBehaviour {

    private GameManager _gameManager;
    private UIManager _uiManager;

    [SerializeField] private CharacterType _characterType;

    private ObjectInteractionListener _currentObjectInteractionListener;
    private GameObject _gameObjectLookingAt;
    private bool reachable;

    [SerializeField] private float _viewDistance;
    [SerializeField] private float _reachDistance;

    // Use this for initialization
    void Start() {
        _gameManager = GameManager.GetGameManager();
        _uiManager = UIManager.GetUiManager();
    }

    // Update is called once per frame
    void Update() {
        UpdateCurrentObject();
        if(_currentObjectInteractionListener == null) {
            _uiManager.ClearUI(_characterType);
        }

        CanCharacterReach();
        
        if(reachable) {
            _currentObjectInteractionListener.KeyInteraction();
            return;
        }
    }

    private void UpdateCurrentObject() {
        Camera characterCamera;
        if(_characterType == CharacterType.Ghost) {
            characterCamera = _gameManager.GhostCamera;
        }
        else if(_characterType == CharacterType.Human) {
            characterCamera = _gameManager.HumanCamera;
        }
        else {
            throw new ArgumentException();
        }

        RaycastHit gameObjectLookingAt;

        var cameraCenter = characterCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, characterCamera.nearClipPlane));

        if(Physics.Raycast(cameraCenter, characterCamera.transform.forward, out gameObjectLookingAt, _viewDistance)) {
            if(gameObjectLookingAt.transform == null) {
                _currentObjectInteractionListener = null;
                _gameObjectLookingAt = null;
                return;
            }

            _gameObjectLookingAt = gameObjectLookingAt.transform.gameObject;
            _currentObjectInteractionListener = _gameObjectLookingAt.GetComponent<ObjectInteractionListener>();
        }
    }

    /// <summary>
    /// Determines if the players are close enough to the object
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private void CanCharacterReach() {
        if(_gameObjectLookingAt == null) return false;
        var character = _gameManager.GetCharacterController(_characterType);
        var characterpos = character.GetComponent<Transform>().position;
        characterpos.y = 0;
        var closesPointToCharacter = _gameObjectLookingAt.GetComponent<Collider>().ClosestPointOnBounds(characterpos);

        if(_characterType == CharacterType.Ghost) {
            if(closesPointToCharacter.y > _gameManager.GhostHeight) return false;
        }
        else if(_characterType == CharacterType.Human) {
            if(closesPointToCharacter.y > _gameManager.HumanHeight) return false;
        }

        closesPointToCharacter.y = 0;
        var distance = Vector3.Distance(characterpos, closesPointToCharacter);
        reachable = distance < _reachDistance;
    }

    //TODO: Move to UI Manager
    private bool ObjectInteractable() {
        if(_characterType == CharacterType.Ghost && _currentObjectInteractionListener.GhostCanOpen == false) return false;
        if(_currentObjectInteractionListener.AnimationType != AnimationType.None) return true;
        if(_currentObjectInteractionListener.ItemName != "") return true;
        if(_currentObjectInteractionListener.Lightswitch) return true;
        return false;
    }

    private void UpdateUi() {
        if(_characterType == CharacterType.Ghost) {
            UpdateGhostUi();
            return;
        }
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

        if(!_ghostReachable) return;

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

        if(_ghostCanOpen && _ghostFlavourText != "") _uiManager.ShowButtons(CharacterType.Ghost, KeyType.B, KeyType.A, GetInstanceID());
    }

}
