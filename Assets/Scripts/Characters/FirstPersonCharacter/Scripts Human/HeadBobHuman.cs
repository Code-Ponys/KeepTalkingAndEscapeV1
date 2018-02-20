using UnityEngine;
using UnityStandardAssets.Utility;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class HeadBobHuman : MonoBehaviour
    {
        public Camera Camera;
        public CurveControlledBobHuman motionBob = new CurveControlledBobHuman();
        public LerpControlledBob jumpAndLandingBob = new LerpControlledBob();
        public RigidbodyFirstPersonControllerHuman RigidbodyFirstPersonControllerHuman;
        public float StrideInterval;
        [Range(0f, 1f)] public float RunningStrideLengthen;

       // private CameraRefocus m_CameraRefocus;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;


        private void Start()
        {
            motionBob.Setup(Camera, StrideInterval);
            m_OriginalCameraPosition = Camera.transform.localPosition;
       //     m_CameraRefocus = new CameraRefocus(Camera, transform.root.transform, Camera.transform.localPosition);
        }


        private void Update()
        {
          //  m_CameraRefocus.GetFocusPoint();
            Vector3 newCameraPosition;
            if (RigidbodyFirstPersonControllerHuman.Velocity.magnitude > 0 && RigidbodyFirstPersonControllerHuman.Grounded)
            {
                Camera.transform.localPosition = motionBob.DoHeadBob(RigidbodyFirstPersonControllerHuman.Velocity.magnitude*(RigidbodyFirstPersonControllerHuman.Running ? RunningStrideLengthen : 1f));
                newCameraPosition = Camera.transform.localPosition;
                newCameraPosition.y = Camera.transform.localPosition.y - jumpAndLandingBob.Offset();
            }
            else
            {
                newCameraPosition = Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - jumpAndLandingBob.Offset();
            }
            Camera.transform.localPosition = newCameraPosition;

            if (!m_PreviouslyGrounded && RigidbodyFirstPersonControllerHuman.Grounded) StartCoroutine(jumpAndLandingBob.DoBobCycle());

            m_PreviouslyGrounded = RigidbodyFirstPersonControllerHuman.Grounded;
          //  m_CameraRefocus.SetFocusPoint();
        }
    }
}
