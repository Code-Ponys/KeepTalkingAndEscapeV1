using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour {

	private float _timer = 3f;
	
	// Update is called once per frame
	void Update () {
		var temp = gameObject.GetComponent<Renderer>().material.color;
		_timer -= Time.deltaTime;

		if(_timer >= 0) {
			temp.a -= 0.38f * Time.deltaTime;
			gameObject.GetComponent<Renderer>().material.color = temp;
		}

		if(_timer <= 0) {
			_timer = 0;
			Destroy(gameObject);
		}
	}
}
