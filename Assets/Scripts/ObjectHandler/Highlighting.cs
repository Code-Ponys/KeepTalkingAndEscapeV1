using System.Collections;
using System.Collections.Generic;
using TrustfallGames.KeepTalkingAndEscape.Manager;
using UnityEngine;

namespace TrustfallGames.KeepTalkingAndEscape.Listener {
	public class Highlighting : MonoBehaviour {

		[SerializeField]private GameObject _ghostHighlight;
		[SerializeField]private GameObject _humanHighlight;
		
		private ObjectInteractionListener _objectInteractionListener;
		private GameManager _gameManager;
		
		private float _currentAxisDelay;
		[SerializeField] private float _axisDelay = 0.5f;
		private List<GameObject> _ghostList = new List<GameObject>();
		private List<GameObject> _humanList = new List<GameObject>();
		private float _ghostTime = 30.0f;
		private float _humanTime = 30.0f;

		private void Start() {
			_gameManager = GameManager.GetGameManager();
		}

		private void Update() {
			ParticleHighlighting();
		}

		public void ParticleHighlighting() {
			Camera ghostCamera = _gameManager.GhostCamera;
			Camera humanCamera = _gameManager.HumanCamera;

			RaycastHit hit;

			var cameraCenter = ghostCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, ghostCamera.nearClipPlane));
			if(Physics.Raycast(cameraCenter, ghostCamera.transform.forward, out hit, 20000)) {
				var distance = hit.distance;
				if(_currentAxisDelay <= 0) {
					if(Input.GetButtonDown(ButtonNames.GhostHighlighting)) {
						foreach(GameObject ghostHighlights in _ghostList) {
							Destroy(ghostHighlights);
						}
						_ghostList.Add(Instantiate(_ghostHighlight, cameraCenter + ghostCamera.transform.forward * distance, hit.transform.rotation));
						_currentAxisDelay = _axisDelay;
					}
				}
				else {
					_currentAxisDelay -= Time.deltaTime;
				}

				if(_ghostList.Count != 0) {
					_ghostTime -= Time.deltaTime;
					if(_ghostTime <= 0.0f) {
						foreach(GameObject ghostHighlights in _ghostList) {
							Destroy(ghostHighlights);
							_ghostTime = 5.0f;
						}
					}
				}
			}
			
			cameraCenter = humanCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, humanCamera.nearClipPlane));
			if(Physics.Raycast(cameraCenter, humanCamera.transform.forward, out hit, 1000)) {
				var distance = hit.distance;
				if(_currentAxisDelay < 0) {
					if(Input.GetButton(ButtonNames.HumanHighlighting)) {
						foreach(GameObject humanHighlights in _humanList) {
							Destroy(humanHighlights);
						}
						_humanList.Add(Instantiate(_humanHighlight, cameraCenter + humanCamera.transform.forward * distance, hit.transform.rotation));
						_currentAxisDelay = _axisDelay;
					}
				}
				else {
					_currentAxisDelay -= Time.deltaTime;
				}
				
				if(_humanList.Count != 0) {
					_humanTime -= Time.deltaTime;
					if(_humanTime <= 0.0f) {
						foreach(GameObject humanHighlights in _humanList) {
							Destroy(humanHighlights);
							_humanTime = 5.0f;
						}
					}
				}
			}
		}
	}
}