using UnityEngine;
using UnityEngine.SceneManagement;

namespace TrustfallGames.KeepTalkingAndEscape.Manager {
    public class LogoScene : MonoBehaviour {
        [SerializeField] private string _goToScene;
        [SerializeField] private float _animationDuration;

        private void Update() {
            _animationDuration -= Time.deltaTime;
            if(_animationDuration < 0)
                SceneManager.LoadScene(_goToScene);
        }
    }
}
