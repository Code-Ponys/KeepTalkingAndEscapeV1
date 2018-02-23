using System.Collections;
using System.Collections.Generic;
using TrustfallGames.KeepTalkingAndEscape.Listener;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	private ObjectInteractionListener _objectInteractionListener;
	private AudioSource _audioSource;
	
	[SerializeField] private AudioClip _pickupSound;

	void Start () {
		_audioSource = gameObject.AddComponent<AudioSource>();
	}
	
	public AudioClip PickupSound {
		get {return _pickupSound;}
	}
	
	public AudioSource Source {
		get {return _audioSource;}
		set {_audioSource = value;}
	}
	
	public static SoundManager GetSoundManager() {
		return GameObject.Find("System").GetComponent<SoundManager>();
	}
}
