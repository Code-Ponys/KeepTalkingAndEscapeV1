using System;
using System.Collections;
using System.Collections.Generic;
using TrustfallGames.KeepTalkingAndEscape.Listener;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour {

	private AudioSource _audioSource;

	[SerializeField] private new bool _dontDestroyOnLoad;
	[SerializeField] private AudioClip _pickupSound;
	[SerializeField] private AudioClip _damageSound;
	[SerializeField] private AudioClip _deathSound;
	[SerializeField] private AudioClip[] _environmentalSounds;
	[SerializeField] private AudioClip _waterSound;
	[SerializeField] private AudioClip _markerSound;
	[SerializeField] private AudioClip _failComboSound;
	[SerializeField] private AudioClip _successComboSound;

	[SerializeField] private float _minRange;
	[SerializeField] private float _maxRange;
	private float _time;
	private int _randomSound;
	private bool _isChoosingSound;


	void Start () {

		if(_dontDestroyOnLoad) {
			DontDestroyOnLoad(gameObject);
		}
		_audioSource = gameObject.AddComponent<AudioSource>();
	}

	private void Update() {
		if(_environmentalSounds.Length == 0) return;
		RandomSounds();
	}


	private void RandomSounds() {
		if(!_isChoosingSound) {
			_isChoosingSound = true;
			_randomSound = Random.Range(0, _environmentalSounds.Length);
			_time = Random.Range(_minRange, _maxRange);
		}

		if(_time <= 0 && _isChoosingSound) {
				_isChoosingSound = false;
				_audioSource.clip = _environmentalSounds[_randomSound];
				_audioSource.Play();
			}
			else if(_isChoosingSound){
				_time -= Time.deltaTime;
			}
		
	}

	public static SoundManager GetSoundManager() {
		return GameObject.Find("SoundManager").GetComponent<SoundManager>();
	}
	
	public AudioClip PickupSound {
		get {return _pickupSound;}
	}
	
	public AudioClip DamageSound {
		get {return _damageSound;}
	}

	public AudioClip DeathSound {
		get {return _deathSound;}
	}

	public AudioClip[] EnvironmentalSounds {
		get {return _environmentalSounds;}
	}

	public AudioClip WaterSound {
		get {return _waterSound;}
	}

	public AudioClip MarkerSound {
		get {return _markerSound;}
	}

	public AudioClip FailComboSound {
		get {return _failComboSound;}
	}

	public AudioClip SuccessComboSound {
		get {return _successComboSound;}
	}
	
	public AudioSource Source {
		get {return _audioSource;}
	}
}
