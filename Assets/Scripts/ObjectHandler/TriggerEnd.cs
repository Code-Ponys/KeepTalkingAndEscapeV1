using TrustfallGames.KeepTalkingAndEscape.Manager;
using UnityEngine;

namespace TrustfallGames.KeepTalkingAndEscape.Listener {
    public class TriggerEnd : MonoBehaviour {
        private UIManager _uiManager;
        [SerializeField] private Sprite _imageHuman;
        [SerializeField] private Sprite _imageGhost;

        private void Start() {
            _uiManager = UIManager.GetUiManager();
        }

        private void OnTriggerEnter(Collider other) {
            Debug.Log("Test");
            _uiManager.TriggerGameOverScreen(true);
            _uiManager.GhostGameOver.sprite = _imageGhost;
            _uiManager.HumanGameOver.sprite = _imageHuman;
        }
    }
}
