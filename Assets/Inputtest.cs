using UnityEngine;

public class Inputtest : MonoBehaviour {

	// Use this for initialization
	private void Start () {
		
	}
	
	// Update is called once per frame
	private void Update () {
			if(Input.GetAxis(ButtonNames.MoveHumanX) < 0)
				Debug.Log("0");
			else if(Input.GetAxis(ButtonNames.MoveHumanX) > 0)
				Debug.Log("1");

		if(Input.GetAxis(ButtonNames.MoveHumanY) < 0)
			Debug.Log("2");
		else if(Input.GetAxis(ButtonNames.MoveHumanY) > 0)
			Debug.Log("3");
	}
}
