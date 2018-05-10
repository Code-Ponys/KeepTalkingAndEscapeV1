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
        if(_gameObjectLookingAt == null) {
            reachable = false;
            return;
        }

        var character = _gameManager.GetCharacterController(_characterType);
        var characterpos = character.GetComponent<Transform>().position;
        characterpos.y = 0;
        var closesPointToCharacter = _gameObjectLookingAt.GetComponent<Collider>().ClosestPointOnBounds(characterpos);

        if(_characterType == CharacterType.Ghost) {
            if(closesPointToCharacter.y > _gameManager.GhostHeight) {
                reachable = false;
                return;
            };
        }
        else if(_characterType == CharacterType.Human) {
            if(closesPointToCharacter.y > _gameManager.HumanHeight) {
                reachable = false;
                return;
            }
        }

        closesPointToCharacter.y = 0;
        var distance = Vector3.Distance(characterpos, closesPointToCharacter);
        reachable = distance < _reachDistance;
    }

    private bool ObjectInteractable() {
        if(_characterType == CharacterType.Ghost && _currentObjectInteractionListener.GhostCanOpen == false) return false;
        if(_currentObjectInteractionListener.AnimationType != AnimationType.None) return true;
        if(_currentObjectInteractionListener.ItemName != "") return true;
        if(_currentObjectInteractionListener.Lightswitch) return true;
        return false;
    }

    private void UpdateUi() {
        SendUiData();
    }

    private void SendUiData() {
        _uiManager.SetCurrentDisplayObject(_currentObjectInteractionListener, reachable, _characterType);
    }

}
