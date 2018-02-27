using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TrustfallGames.KeepTalkingAndEscape.Manager {
	public class StartMenu : MonoBehaviour {

		[SerializeField] private GameObject[] _menu;
		[SerializeField] private string[] _scene;

		[SerializeField] private float _axisDelay = 0.5f;
		[SerializeField] private CharacterType _characterType;
		private bool _menuActive;
		private float _currentAxisDelay;
		private static int _currentPressedButtonHuman;
		private static int _currentPressedButtonGhost;

		[SerializeField] private Sprite _outline;
		[SerializeField] private Sprite _defaultSprite;

		private UIManager _uiManager;
		private GameManager _gameManager;

		private int _y = 0;

		// Use this for initialization
		void Start() {
			
			_uiManager = UIManager.GetUiManager();
			_gameManager = GameManager.GetGameManager();

			var canvas = gameObject.GetComponent<Canvas>();
			_currentPressedButtonHuman = 5;
			_currentPressedButtonGhost = 5;  

			switch(_characterType) {
				case CharacterType.Ghost:
					canvas.renderMode = RenderMode.ScreenSpaceCamera;
					canvas.worldCamera = _gameManager.GhostCamera;
					break;
				case CharacterType.Human:
					canvas.renderMode = RenderMode.ScreenSpaceCamera;
					canvas.worldCamera = _gameManager.HumanCamera;
					break;
			}
		}

		// Update is called once per frame
		void Update() {
			ToggleMenuVisible();
			if(!_menuActive) return;
			
			UpdateSelection();
			StartMenuInput();
			Clicked();
			StartMenuMenu();
		}

		/// <summary>
		/// Toggles Inventory visibility. Rearranges the items, if the other inventory is closed
		/// </summary>
		private void ToggleMenuVisible() {
			Debug.Log("Test");
			switch(_characterType) {
				case CharacterType.Ghost:
					if(Input.GetButtonDown(ButtonNames.GhostMenu)) {
						Debug.Log("Open Menu");
						_menuActive = true;
						_uiManager.TriggerMenu(true,CharacterType.Ghost);
						_y = 0;
					}

					break;
				case CharacterType.Human:
					if(Input.GetButtonDown(ButtonNames.HumanMenu)) {
						Debug.Log("Open Menu");
						_menuActive = true;
						_uiManager.TriggerMenu(true,CharacterType.Human);
						_y = 0;
					}

					break;
			}
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

		private void StartMenuInput() {
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
									_currentPressedButtonHuman = _y;
									_currentAxisDelay = _axisDelay;
								}

								if(_menu[1]) {
									_currentPressedButtonHuman = _y;
									_currentAxisDelay = _axisDelay;
								}

								if(_menu[2]) {
									_currentPressedButtonHuman = _y;
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
									_currentPressedButtonGhost = _y;
									_currentAxisDelay = _axisDelay;
								}

								if(_menu[1]) {
									_currentPressedButtonGhost = _y;
									_currentAxisDelay = _axisDelay;
								}

								if(_menu[2]) {
									_currentPressedButtonGhost = _y;
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

		private void StartMenuMenu() {
			if(_currentPressedButtonHuman == 0 || _currentPressedButtonGhost == 0) {
				switch(_characterType) {
						case CharacterType.Ghost:
							_menuActive = false;
							_uiManager.TriggerMenu(false, CharacterType.Ghost);

							_currentPressedButtonGhost = 5;
							break;
						case CharacterType.Human:
							_menuActive = false;
							_uiManager.TriggerMenu(false, CharacterType.Human);
							_currentPressedButtonHuman = 5;
							break;
				}
			}
			if(_currentPressedButtonHuman == 1 && _currentPressedButtonGhost == 1) {
				SceneManager.LoadScene(_scene[0]);
			}

			if(_currentPressedButtonHuman == 2 && _currentPressedButtonGhost == 2) {
				Application.Quit();
			}
		}
		
		public bool MenuActive {
			get {return _menuActive;}
		}
		
		public static StartMenu GetInstance(CharacterType characterType) {
			if(characterType == CharacterType.Ghost) return GameObject.Find("GhostMenu").GetComponent<StartMenu>();

			return GameObject.Find("HumanMenu").GetComponent<StartMenu>();
		}
	}
}
