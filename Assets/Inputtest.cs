using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputtest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	private void Update () {
			if(Input.GetAxis(ButtonNames.MoveHumanX) < 0) {
				//Left
				Debug.Log("0");
			}
			else if(Input.GetAxis(ButtonNames.MoveHumanX) > 0) {
				//Right
				Debug.Log("1");
			}

			if(Input.GetAxis(ButtonNames.MoveHumanY) < 0) {
				//Down
				Debug.Log("2");
			}
			else if(Input.GetAxis(ButtonNames.MoveHumanY) > 0) {
				//Up
				Debug.Log("3");
			}
	}
}
