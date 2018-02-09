using UnityEngine;
using System;
using NUnit.Framework;
using TrustfallGames.KeepTalkingAndEscape.Listener;

namespace KeepTalkingAndEscape {
    [TestFixture]
    public class AnimationControllerTest {
        [Test]
        public void StepsPerFrameTest() {
            AnimationController animationController = new AnimationController();

            Vector3 a = new Vector3(270, 0, 0);
            Vector3 b = new Vector3(-89, 0, 0);

            Vector3 result = animationController.StepsPerFrame(a, b, 1);


            if(result.x > 0) {
                Assert.Fail();
            }
        }
    }
}
