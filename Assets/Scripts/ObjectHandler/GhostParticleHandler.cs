using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostParticleHandler : MonoBehaviour {
	
	[SerializeField] private GameObject _meshGameObject;
	[SerializeField] private Material _material;

	private ParticleSystem _particleSystem;

	// Use this for initialization
	void Start () {
		ParticleHandler();
	}

	private void ParticleHandler() {
		
		GameObject _particleObject = new GameObject("Particle");
		_particleObject.transform.parent = _meshGameObject.transform;
		_particleObject.transform.position = _meshGameObject.transform.position;
		_particleObject.layer = 4;

		_particleSystem = _particleObject.AddComponent<ParticleSystem>();
		_particleObject.GetComponent<Renderer>().material = _material;
	}
}
