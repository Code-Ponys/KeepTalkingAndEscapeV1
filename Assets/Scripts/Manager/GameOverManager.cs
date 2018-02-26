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
			var gameOverManager = GetComponent<GameOverManager>();
			gameOverManager.enabled = false;
			_currentPressedButtonHuman = 5;
			_currentPressedButtonGhost = 5;   
		}

		// Update is called once per frame
		void Update() {
			if(_objectInteractionListener.IsGameOver)
			GameOverEnabler();

			
		}

		private void GameOverEnabler() {
			_uiManager.HumanGameOver.GetComponent<Text>().enabled = true;
			_uiManager.HumanMainMenuButton.GetComponent<Text>().enabled = true;
			_uiManager.HumanReplayButton.GetComponent<Text>().enabled = true;
			_uiManager.HumanQuitMenu.GetComponent<Text>().enabled = true;
			_uiManager.GhostGameOver.GetComponent<Text>().enabled = true;
			_uiManager.GhostMainMenuButton.GetComponent<Text>().enabled = true;
			_uiManager.GhostReplayButton.GetComponent<Text>().enabled = true;
			_uiManager.GhostQuitMenu.GetComponent<Text>().enabled = true;
			Time.timeScale = 0;
			var gameOverManager = GetComponent<GameOverManager>();
			gameOverManager.enabled = true;	
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
					if(_currentAxisDelay < 0) {
						if(Input.GetAxis(ButtonNames.MoveHumanY) < 0) {
							//Up
							if(_y == 3) return;
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
					if(_currentAxisDelay < 0) {
						if(Input.GetAxis(ButtonNames.MoveGhostY) < 0) {
							//Up
							Debug.Log("Pressed Down");
							if(_y == 3) return;
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
			
		}
	}
}
