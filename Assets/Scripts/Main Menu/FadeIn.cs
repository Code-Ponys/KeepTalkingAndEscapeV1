using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrustfallGames.KeepTalkingAndEscape.Manager {
	public class FadeIn : MonoBehaviour {

		[SerializeField] private bool _fadeIn;
		private float _timer = 3f;

		// Update is called once per frame
		void Update() {
			var temp = gameObject.GetComponent<Renderer>().material.color;
			_timer -= Time.deltaTime;
			//Begins Fade In
			if(_timer >= 0 && _fadeIn) {
				temp.a -= 0.38f * Time.deltaTime;
				gameObject.GetComponent<Renderer>().material.color = temp;
			}

			if(_timer <= 0 && !_fadeIn) {
				temp.a += 0.38f * Time.deltaTime;
				gameObject.GetComponent<Renderer>().material.color = temp;
			}

			if(_timer <= 0) {
				_timer = 0;
				Destroy(gameObject);
			}
		}

		public static FadeIn GetFadeIn() {
			return GameObject.Find("Panel").GetComponent<FadeIn>();
		}
	}
}
