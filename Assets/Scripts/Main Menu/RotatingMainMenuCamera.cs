using UnityEngine;

public class RotatingMainMenuCamera : MonoBehaviour {
	
	// Update is called once per frame
	private void Update () {
		transform.RotateAround(transform.position, transform.up, Time.deltaTime * 10f);
	}
}
