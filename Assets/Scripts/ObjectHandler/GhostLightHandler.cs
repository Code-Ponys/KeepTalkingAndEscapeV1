using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostLightHandler : MonoBehaviour {
	
	[SerializeField] private GameObject _meshGameObject;
	[SerializeField] private Material _material;
	
	[SerializeField] private float _lightRange = 30;
	[SerializeField] private float _lightIntensity = 10;
	[SerializeField] private float _lightPositionX = 0;
	[SerializeField] private float _lightPositionY = 0;
	[SerializeField] private float _lightPositionZ = 0;
	
	[SerializeField] private Color _lightColor;

	private Light _light;

	// Use this for initialization
	void Start () {
		LightHandler();
	}
	
	private void LightHandler() {
		
		GameObject lightObject = new GameObject("Light");
		lightObject.transform.parent = _meshGameObject.transform;
		lightObject.transform.position = new Vector3(_meshGameObject.transform.position.x + _lightPositionX, _meshGameObject.transform.position.y + _lightPositionY, _meshGameObject.transform.position.z + _lightPositionZ);
		lightObject.transform.rotation = _meshGameObject.transform.rotation;
		lightObject.transform.localScale = _meshGameObject.transform.localScale;
		lightObject.layer = 9;

		_light = lightObject.AddComponent<Light>();
		var lightMaster = _light;

		lightMaster.type = LightType.Point;
		lightMaster.range = _lightRange;
		lightMaster.intensity = _lightIntensity;
		lightMaster.color = _lightColor;
	}
}