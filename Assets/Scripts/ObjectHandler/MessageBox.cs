using UnityEngine;

namespace TrustfallGames.KeepTalkingAndEscape.Listener {
    public class MessageBox : MonoBehaviour {
        [SerializeField] private float _playMessageAfterSeconds;
        [SerializeField] private AudioClip _message;
        [SerializeField] private SoundManager _soundManager;
        private bool _played;


        private void Start() {
            _soundManager = SoundManager.GetSoundManager();
        }

        private void Update() {
            if(_played) return;
            if(_playMessageAfterSeconds < 0) {
                _played = true;
                _soundManager.Source.clip = _message;
            }
            else {
                _playMessageAfterSeconds -= Time.deltaTime;
            }
        }
    }
}
