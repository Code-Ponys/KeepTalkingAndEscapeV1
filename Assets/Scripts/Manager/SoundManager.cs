using System.Collections;
using System.Collections.Generic;
using TrustfallGames.KeepTalkingAndEscape.Listener;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	private ObjectInteractionListener _objectInteractionListener;
	private AudioSource _audioSource;
	
	[SerializeField] private AudioClip _pickupSound;
	[SerializeField] private AudioClip _damageSound;
	[SerializeField] private AudioClip _deathSound;
	
	void Start () {
		_audioSource = gameObject.AddComponent<AudioSource>();
	}
	
	public static SoundManager GetSoundManager() {
		return GameObject.Find("System").GetComponent<SoundManager>();
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
	
	public AudioSource Source {
		get {return _audioSource;}
		set {_audioSource = value;}
	}
	
}
