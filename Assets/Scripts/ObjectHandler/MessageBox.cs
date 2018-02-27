using UnityEngine;

namespace TrustfallGames.KeepTalkingAndEscape.Listener {
    public class MessageBox : MonoBehaviour {
        [SerializeField] private float _playMessageAfterSeconds;
        [SerializeField] private AudioClip _message;
        private AudioSource _source;
        private bool _played;


        private void Start() {
            _source = gameObject.AddComponent<AudioSource>();
            _source.maxDistance = 5000;
        }

        private void Update() {
            if(_played) return;
            if(_playMessageAfterSeconds < 0) {
                _played = true;
                _source.clip = _message;
                _source.Play();
            }
            else {
                _playMessageAfterSeconds -= Time.deltaTime;
            }
        }
    }
}
