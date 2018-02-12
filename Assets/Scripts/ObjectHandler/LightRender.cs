using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRender : MonoBehaviour {

	private Light _light;

	// Use this for initialization
	void Start () {
		OnPreCull();
	}

	private void OnPreCull() {
		_light.enabled = false;
	}
}	
