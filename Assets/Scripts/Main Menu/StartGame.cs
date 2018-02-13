using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TrustfallGames.KeepTalkingAndEscape.Manager {
	public class StartGame : MonoBehaviour {

		private bool HumanPressedButton = false;
		private bool GhostPressedButton = false;

		[SerializeField] private CharacterType _characterType;

		// Use this for initialization
		void Start() {

		}

		// Update is called once per frame
		void Update() {

		}

		private void Play() {
			switch(_characterType) {
				case CharacterType.Human:
					if(Input.GetButton(ButtonNames.HumanInspect) && HumanPressedButton != true) {
						HumanPressedButton = true;
						Debug.Log("Is Human Button pressed: " + HumanPressedButton);
					}
					else if(Input.GetButton(ButtonNames.HumanInspect) && HumanPressedButton) {
						HumanPressedButton = false;
						Debug.Log("Is Human Button pressed: " + HumanPressedButton);
					}

					break;

				case CharacterType.Ghost:
					if(Input.GetButton(ButtonNames.GhostInspect) && GhostPressedButton != true) {
						GhostPressedButton = true;
						Debug.Log("Is Ghost Button pressed: " + GhostPressedButton);
					}
					else if(Input.GetButton(ButtonNames.GhostInspect) && GhostPressedButton) {
						GhostPressedButton = false;
						Debug.Log("Is Ghost Button pressed: " + GhostPressedButton);
					}

					break;
			}
		}
	}
}
