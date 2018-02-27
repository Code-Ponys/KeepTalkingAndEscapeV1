using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TrustfallGames.KeepTalkingAndEscape.Manager {
	public class FadeOut : MonoBehaviour {

		private GameObject _panel;
		private float _timer = 3f;

		private void Start() {
			StartCoroutine("FadeIn");
		}

		// Update is called once per frame
		void Update() {
			var temp = gameObject.GetComponent<MeshRenderer>().material.color;
			_timer -= Time.deltaTime;
			//Begins Fade Out
			if(_timer >= 0) {
				temp.a -= 0.38f * Time.deltaTime;
				gameObject.GetComponent<MeshRenderer>().material.color = temp;
			}

			if(_timer <= 0) {
				_timer = 0;
				Destroy(gameObject);
			}
		}
	}
}

