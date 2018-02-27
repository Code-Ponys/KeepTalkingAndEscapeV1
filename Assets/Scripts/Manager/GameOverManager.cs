using TrustfallGames.KeepTalkingAndEscape.Listener;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TrustfallGames.KeepTalkingAndEscape.Manager {
	public class GameOverManager : MonoBehaviour {

		private GameManager _gameManager;
		private UIManager _uiManager;
		private ObjectInteractionListener _objectInteractionListener;

		[SerializeField] private GameObject[] _menu;
		[SerializeField] private string[] _scene;
		
		[SerializeField] private float _axisDelay = 0.5f;
		[SerializeField] private CharacterType _characterType;
		
		private float _currentAxisDelay;
		private static int _currentPressedButtonHuman;
		private static int _currentPressedButtonGhost;
		
		[SerializeField] private Sprite _outline;
		[SerializeField] private Sprite _defaultSprite;
		
		private int _y = 0;

		// Use this for initialization
		void Start() {
			_gameManager = GameManager.GetGameManager();
			_uiManager = UIManager.GetUiManager();
			_currentPressedButtonHuman = 5;
			_currentPressedButtonGhost = 5;
		}

		// Update is called once per frame
		void Update() {
			if(_gameManager.HumanController.Health <= 0) {
				GameOverEnabler();
			}

			if(_gameManager.HumanController.Health <= 0) {
				UpdateSelection();
				GameOverInput();
				Clicked();
				GameOverMenu();
			}
		}

		private void GameOverEnabler() {
			_uiManager.TriggerGameOverScreen(true);
//			Time.timeScale = 0;
		}
		
		private void UpdateSelection() {
			foreach(var obj in _menu) obj.GetComponent<Image>().sprite = _defaultSprite;

			foreach(var obj in _menu) obj.GetComponent<Image>().sprite = _defaultSprite;
			switch(_characterType) {
				case CharacterType.Ghost:
					_menu[_y].GetComponent<Image>().sprite = _outline;
					break;
				case CharacterType.Human:
					_menu[_y].GetComponent<Image>().sprite = _outline;
					break;
			}
		}
		
		private void GameOverInput() {
			//Button input for human
			switch(_characterType) {
				case CharacterType.Human:
					//Change current button
					if(_currentAxisDelay <= 0) {
						if(Input.GetAxis(ButtonNames.MoveHumanY) < 0) {
							//Up
							if(_y == 2) return;
							_y++;
							_currentAxisDelay = _axisDelay;
						}
						else if(Input.GetAxis(ButtonNames.MoveHumanY) > 0) {
							//Down
							if(_y == 0) return;
							_y--;
							_currentAxisDelay = _axisDelay;
						}
					}
					else {
						_currentAxisDelay -= Time.deltaTime;
					}
					break;
				case CharacterType.Ghost:
					//Change current button
					if(_currentAxisDelay <= 0) {
						if(Input.GetAxis(ButtonNames.MoveGhostY) < 0) {
							//Up
							Debug.Log("Pressed Down");
							if(_y == 2) return;
							_y++;
							_currentAxisDelay = _axisDelay;
						}

						if(Input.GetAxis(ButtonNames.MoveGhostY) > 0) {
							//Down
							Debug.Log("Pressed Up");
							if(_y == 0) return;
							_y--;
							_currentAxisDelay = _axisDelay;
						}
					}
					else {
						_currentAxisDelay -= Time.deltaTime;
					}
					break;
			}
		}
		
		public void Clicked() {
            switch(_characterType) {
                case CharacterType.Human:
                    if(_currentAxisDelay <= 0) {
                        if(Input.GetButtonDown(ButtonNames.HumanInspect))
                            if(_menu[_y] != null) {
                                if(_menu[0]) {
                                    _currentPressedButtonHuman =_y;
                                    _currentAxisDelay = _axisDelay;
                                }
                                
                                if(_menu[1]) {
                                    _currentPressedButtonHuman =_y;
                                    _currentAxisDelay = _axisDelay;
                                }
                                
                                if(_menu[2]) {
                                    _currentPressedButtonHuman =_y;
                                    _currentAxisDelay = _axisDelay;
                                }
                            }
                    }
                    else {
                        _currentAxisDelay -= Time.deltaTime;
                    }
                    break;

                case CharacterType.Ghost:
                    if(_currentAxisDelay < 0) {
                       if(Input.GetButtonDown(ButtonNames.GhostInspect))
                           if(_menu[_y] != null) {
                               if(_menu[0]) {
                                   _currentPressedButtonGhost =_y;
                                   _currentAxisDelay = _axisDelay;
                               }
                                
                               if(_menu[1]) {
                                   _currentPressedButtonGhost =_y;
                                   _currentAxisDelay = _axisDelay;
                               }
                                
                               if(_menu[2]) {
                                   _currentPressedButtonGhost =_y;
                                   _currentAxisDelay = _axisDelay;
                               }
                           }
                    }
                    else {
                        _currentAxisDelay -= Time.deltaTime;
                    }
                    break;
            }
        }

		private void GameOverMenu() {
			if(_currentPressedButtonHuman == 0 && _currentPressedButtonGhost == 0) {
				SceneManager.LoadScene(_scene[0]);
			}
			if(_currentPressedButtonHuman == 1 && _currentPressedButtonGhost == 1) {
				SceneManager.LoadScene(_scene[1]);
			}
			if(_currentPressedButtonHuman == 2 && _currentPressedButtonGhost == 2) {
				Application.Quit();
			}
		}
	}
}
