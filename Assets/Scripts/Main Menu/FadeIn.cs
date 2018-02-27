using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TrustfallGames.KeepTalkingAndEscape.Manager {
    public class FadeIn : MonoBehaviour {

        private Image _fadeRenderer;
        private bool _faded;

        private void Start() {
            _fadeRenderer = gameObject.GetComponent<Image>();
//            var temp = _fadeRenderer.material.color;
//            temp.a = 255f;
//            _fadeRenderer.material.color = temp;
        }

        // Update is called once per frame
        void Update() {
            if(_faded) return;
            var temp = _fadeRenderer.material.color;
            //Begins Fade Out
            temp.a -= 0.001f;
            _fadeRenderer.material.color = temp;
            if(temp.a == 0)
                _faded = true;
        }
    }
}