using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSound : MonoBehaviour {
	private AudioSource _audioSource;
	
	[SerializeField] private AudioClip _backgroundSounds;

	// Use this for initialization
	void Start () {
		_audioSource = gameObject.AddComponent<AudioSource>();
		if(_backgroundSounds != null) {
			_audioSource.clip = _backgroundSounds;
			_audioSource.maxDistance = 5000;
			_audioSource.minDistance = 4999;
			_audioSource.Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(_backgroundSounds != null) {
			_audioSource.loop = true;
		}
	}
}
