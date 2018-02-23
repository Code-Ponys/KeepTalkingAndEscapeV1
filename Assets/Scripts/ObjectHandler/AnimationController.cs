using System;
using UnityEngine;

namespace TrustfallGames.KeepTalkingAndEscape.Listener {
    public class AnimationController : MonoBehaviour {
        private ObjectInteractionListener _objectInteractionListener;
        private GameObject _meshGameObject;
        private GameObject _linkedMeshGameObject;

        private Vector3 _positionBase;
        private Quaternion _rotationBase;
        private Vector3 _scaleBase;

        //Steps
        private Vector3 _positionStepOpen;
        private Quaternion _rotationStepOpen;
        private Vector3 _scaleStepOpen;
        private Vector3 _positionStepClose;
        private Quaternion _rotationStepClose;
        private Vector3 _scaleStepClose;

        //Current Position
        private Vector3 _positionCurrent;
        private Quaternion _rotationCurrent;

        private Vector3 _scaleCurrent;

        //Animated Goal Position
        private Vector3 _positionAnimated;
        private Quaternion _rotationAnimated;
        private Vector3 _scaleAnimated;

        private Rigidbody _rigidbody;
        private AnimationType _animationType;
        private KeyType _keyType;
        private int _animationDurationInFrames;
        private int _frameCount = 0;
        private int _framesToNextStop = 0;
        private int _animationStepsPerKlick;

        private float _rotationSteps;

        private bool _animationActive;
        private bool _animationActivated;
        private bool _open;
        private bool _dataRead;
        private bool _ghostDrivenAnimationActive = false;
        private bool _onedirectionAnimation = false;
        private bool _used = false;
        private bool _activateObjectPhysikAfterAnimation;
        private bool _parentAnimationDone;
        private bool _childAnimationActive;
        private bool _childAnimationOpen;
        private bool _childAnimationDone;
      

        //Update
        private void FixedUpdate() {
            if(_linkedMeshGameObject != null) linkedAnimation();

            ActivateChildOnHold();
            if(_childAnimationActive && _childAnimationOpen) {
                _objectInteractionListener.Source.clip = _objectInteractionListener.CloseSound;
                _objectInteractionListener.Source.Play();
            }
            else
            if(_childAnimationActive && !_childAnimationOpen && _objectInteractionListener.Source.clip != _objectInteractionListener.OpenSound) {
                _objectInteractionListener.Source.clip = _objectInteractionListener.OpenSound;
                _objectInteractionListener.Source.Play();
            }
            //Sound for the Record player
//            if(_animationType == AnimationType.GhostActivateOnKeyHold && _ghostDrivenAnimationActive && _objectInteractionListener.IsARadio) {
//                _objectInteractionListener.Source.clip = _objectInteractionListener.RadioSound;
//                _objectInteractionListener.Source.loop = true;
//                _objectInteractionListener.Source.Play();
//            }
//            else if(!_ghostDrivenAnimationActive && _objectInteractionListener.IsARadio){
//                _objectInteractionListener.Source.Pause();
//            }
            if(!_animationActive) return;
            OpenAnimation();
            MoveOnKeySmash();
        }


//Animation Types

        /// <summary>
        /// Operates opening of objects. Moves the object to a specific position
        /// </summary>
        private void OpenAnimation() {
            if(_animationType != AnimationType.Open) return;
            if(_open) {
                //close Door
                if(_frameCount == _animationDurationInFrames) {
                    SetObjectToPos(_positionBase, _rotationBase, _scaleBase);
                    _open = false;
                    _animationActive = false;
                    _frameCount = 0;
                    Debug.Log("Door closed");
                    return;
                }

                TransformObject(_positionStepClose, _scaleStepClose);
            }
            else {
                //Open Door
                if(_frameCount == _animationDurationInFrames) {
                    SetObjectToPos(_positionAnimated, _rotationAnimated, _scaleAnimated);
                    _open = true;
                    _animationActive = false;
                    _frameCount = 0;
                    Debug.Log("Door opened");
                    return;
                }

                //close Door
                TransformObject(_positionStepOpen, _scaleStepOpen);
            }
        }

        /// <summary>
        /// Moves object by pressing the same button consecutively
        /// </summary>
        private void MoveOnKeySmash() {
            if(GhostDrivenAnimationActive)
                if(_animationType == AnimationType.GhostMoveOnKeySmash) {
                    //Button, which sould be smashed
                    if(Input.GetButtonDown(ButtonNames.GetButtonName(_keyType)))
                        if(_framesToNextStop == 0 && _animationDurationInFrames != _frameCount) _framesToNextStop = _animationStepsPerKlick;

                    //If there are frames left, he proceeds the animation
                    if(_framesToNextStop != 0 && _animationDurationInFrames != _frameCount) {
                        _framesToNextStop--;
                        TransformObject(_positionStepOpen, _scaleStepOpen);
                    }
                    //Checks if animation is processed
                    else if(_animationDurationInFrames == _frameCount) {
                        _framesToNextStop = 0;
                        _ghostDrivenAnimationActive = false;
                        if(_objectInteractionListener.CanBeTakenToInventory) _objectInteractionListener.CanBePickedUpAfterGhostAction = false;

                        if(_activateObjectPhysikAfterAnimation) {
                            _rigidbody.useGravity = true;
                            _rigidbody.isKinematic = false;
                            _animationActive = false;
                        }

                        _open = true;
                        SetObjectToPos(_positionAnimated, _rotationAnimated, _scaleBase);
                    }
                }

            if(_open && !_onedirectionAnimation) {
            }
        }

        /// <summary>
        /// Controls the animation for two GameObjects, that are linked with each other
        /// </summary>
        private void linkedAnimation() {
            if(LinkedAnimationActive())
                if(!_childAnimationOpen && !_childAnimationActive) {
                    _childAnimationActive = true;
                    _frameCount = 0;
                }

            if(!LinkedAnimationActive())
                if(_childAnimationOpen && _childAnimationActive) {
                    _frameCount = 0;
                    _childAnimationActive = false;
                }

            if(_childAnimationActive && !_childAnimationOpen) {
                //Open Door
                if(_frameCount == _animationDurationInFrames) {
                    Debug.Log("Open Door");
                    SetObjectToPos(_positionAnimated, _rotationAnimated, _scaleAnimated);
                    _childAnimationOpen = true;
                    _open = true;
                    return;
                }

                TransformObject(_positionStepOpen, _scaleStepOpen);
            }

            if(_childAnimationActive || !_childAnimationOpen) return;
            //close Door
            if(_frameCount == _animationDurationInFrames) {
                Debug.Log("CloseDoor");
                SetObjectToPos(_positionBase, _rotationBase, _scaleBase);
                _childAnimationOpen = false;
                _open = false;
                return;
            }

            TransformObject(_positionStepClose, _scaleStepClose);
        }

        /// <summary>
        /// Determines if the animation for LinkedAnimation is active
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private bool LinkedAnimationActive() {
            if(_linkedMeshGameObject.GetComponent<ObjectInteractionListener>().ActivateChildWhen == ActivateChildWhen.ButtonPressed) return _linkedMeshGameObject.GetComponent<ObjectInteractionListener>().IsHumanPressingAgainsObject();

            if(_linkedMeshGameObject.GetComponent<ObjectInteractionListener>().ActivateChildWhen == ActivateChildWhen.AnimationDone) return _linkedMeshGameObject.GetComponent<AnimationController>().ParentAnimationDone;

            throw new ArgumentException("Linked Animation failed");
        }

        /// <summary>
        /// Part of the linked Animation. Hold a button to trigger the animation
        /// </summary>
        private void ActivateChildOnHold() {
            //Abbrechen

            if(Input.GetButtonDown(ButtonNames.GetButtonName(KeyType.A))) {
                _ghostDrivenAnimationActive = false;
            }

            if(_animationType != AnimationType.GhostActivateOnKeyHold) return;
            if(_ghostDrivenAnimationActive)
                if(Input.GetButton(ButtonNames.GetButtonName(_keyType)) && !_animationActivated) {
                    _animationActivated = true;
                    _frameCount = 0;
                }

            if(!_ghostDrivenAnimationActive || !Input.GetButton(ButtonNames.GetButtonName(_keyType)))
                if(_open && _animationActivated) {
                    _frameCount = 0;
                    _animationActivated = false;
                }

            if(_animationActivated && !_open) {
                if(_frameCount == _animationDurationInFrames) {
                    SetObjectToPos(_positionAnimated, _rotationAnimated, _scaleAnimated);
                    _open = true;
                }

                TransformObject(_positionStepOpen, _scaleStepOpen);
            }

            if(_animationActivated || !_open) return;
            if(_frameCount == _animationDurationInFrames) {
                SetObjectToPos(_positionBase, _rotationBase, _scaleBase);
                _animationActive = false;
                _open = false;
            }

        TransformObject(_positionStepClose, _scaleStepClose);
        }
//End of Animation Type


//Methodes for moving 

        /// <summary>
        /// Moves the object to a point determined in the editor
        /// </summary>
        /// <param name="positionStep"></param>
        /// <param name="rotationStep"></param>
        /// <param name="scaleStep"></param>
        private void TransformObject(Vector3 positionStep, Vector3 scaleStep) {
            //Update PositionData
            _positionCurrent = _meshGameObject.transform.localPosition;
            _rotationCurrent = _meshGameObject.transform.localRotation;
            _scaleCurrent = _meshGameObject.transform.localScale;
            //Transform
            _meshGameObject.transform.localPosition = new Vector3(_positionCurrent.x + positionStep.x, _positionCurrent.y + positionStep.y, _positionCurrent.z + positionStep.z);
            _meshGameObject.transform.localRotation = _open ? Quaternion.RotateTowards(_rotationAnimated, _rotationBase, _rotationSteps * _frameCount) : Quaternion.RotateTowards(_rotationBase, _rotationAnimated, _rotationSteps * _frameCount);

            _meshGameObject.transform.localScale = new Vector3(_scaleCurrent.x + scaleStep.x, _scaleCurrent.y + scaleStep.y, _scaleCurrent.z + scaleStep.z);
            _frameCount++;
        }

        private void SetObjectToPos(Vector3 position, Quaternion rotation, Vector3 scale) {
            _meshGameObject.transform.localPosition = position;
            _meshGameObject.transform.localRotation = rotation;
            _meshGameObject.transform.localScale = scale;
        }
//End of Methodes for Moving


//New animation methode

        /// <summary>
        /// Reverses Rotation values for animation and activates animations
        /// </summary>
        /// <param name="self"></param>
        /// <exception cref="ArgumentException"></exception>
        public void StartNewAnimation(ObjectInteractionListener self) {
            _objectInteractionListener = self;
            _meshGameObject = _objectInteractionListener.MeshGameObject;
            if(_animationType == AnimationType.Open && _dataRead != true) {
                WriteData(self);
                _dataRead = true;
            }

            if(_animationType != AnimationType.Open) WriteData(self);

            validateData(self);

            CalculateSteps();

            _dataRead = true;

            if(_animationActive && _animationType == AnimationType.Open) return;

            if(_activateObjectPhysikAfterAnimation) {
                if(_meshGameObject.GetComponent<Rigidbody>() == null) _meshGameObject.AddComponent<Rigidbody>();

                _rigidbody = _meshGameObject.GetComponent<Rigidbody>();
                if(_rigidbody.useGravity == true) _rigidbody.useGravity = false;

                _rigidbody.isKinematic = true;
            }

            var a = _meshGameObject.GetComponent<Rigidbody>();

            if(_animationType == AnimationType.GhostMoveOnKeySmash || _animationType == AnimationType.GhostActivateOnKeyHold)
                _ghostDrivenAnimationActive = true;

            if(!_open && _animationType == AnimationType.GhostActivateOnKeyHold) _frameCount = 0;

            if(_onedirectionAnimation && _animationType == AnimationType.GhostMoveOnKeySmash)
                _frameCount = 0;
            if(_keyType == KeyType.A && _animationType == AnimationType.GhostMoveOnKeySmash) throw new ArgumentException("Key Type can not be A for a GhostMoveOnSmash animation");

            _animationActive = true;
            if(_open)
            {
                _objectInteractionListener.Source.clip = _objectInteractionListener.CloseSound;
            }
            else {
                _objectInteractionListener.Source.clip = _objectInteractionListener.OpenSound;
            }
            _objectInteractionListener.Source.Play();
        }

        /// <summary>
        /// Calculate the step size for the frame duration.
        /// </summary>
        private void CalculateSteps() {
            _positionStepOpen = StepsPerFrame(_positionBase, _positionAnimated, _animationDurationInFrames);
            _positionStepClose = _positionStepOpen * -1;
            CalculateRotationSteps(_rotationBase, _rotationAnimated, _animationDurationInFrames);
            _scaleStepOpen = StepsPerFrame(_scaleBase, _scaleAnimated, _animationDurationInFrames);
            _scaleStepClose = _scaleStepOpen * -1;
        }

        /// <summary>
        /// Validates the given data to make 0 input possible.
        /// </summary>
        /// <param name="self"></param>
        private void validateData(ObjectInteractionListener self) {
            if(_positionAnimated.x == 0) _positionAnimated.x = _positionBase.x;

            if(_positionAnimated.y == 0) _positionAnimated.y = _positionBase.y;

            if(_positionAnimated.z == 0) _positionAnimated.z = _positionBase.z;

            _rotationAnimated = Quaternion.Euler(self.RotationAnimated);

            _scaleAnimated = self.ScaleAnimated;
            if(_scaleAnimated.x == 0) _scaleAnimated.x = _scaleBase.x;

            if(_scaleAnimated.y == 0) _scaleAnimated.y = _scaleBase.y;

            if(_scaleAnimated.z == 0) _scaleAnimated.z = _scaleBase.z;
        }

        /// <summary>
        /// Forms the link between GameObject and the animation
        /// </summary>
        /// <param name="linkedGameObject"></param>
        /// <param name="self"></param>
        public void StartNewAnimation(GameObject linkedGameObject, ObjectInteractionListener self) {
            _objectInteractionListener = self;
            if(_childAnimationActive || _childAnimationOpen) return;
            _frameCount = 0;
            WriteData(self);

            validateData(self);


            CalculateSteps();

            _dataRead = true;


            if(_animationActive && _animationType == AnimationType.Open) return;

            if(_activateObjectPhysikAfterAnimation) {
                if(_meshGameObject.GetComponent<Rigidbody>() == null) _meshGameObject.AddComponent<Rigidbody>();

                _rigidbody = _meshGameObject.GetComponent<Rigidbody>();
                if(_rigidbody.useGravity == true) _rigidbody.useGravity = false;

                _rigidbody.isKinematic = true;
            }

            var a = _meshGameObject.GetComponent<Rigidbody>();

            _linkedMeshGameObject = linkedGameObject;
        }

//End of new animation Methodes

//Calculations

        /// <summary>
        /// Writes the data of the given Object listener to the Animation Controller.
        /// </summary>
        /// <param name="self"></param>
        private void WriteData(ObjectInteractionListener self) {
            _meshGameObject = self.MeshGameObject;
            _positionBase = _meshGameObject.transform.localPosition;
            _rotationBase = _meshGameObject.transform.localRotation;
            _scaleBase = _meshGameObject.transform.localScale;
            _animationStepsPerKlick = self.AnimationStepsPerKlick;
            _positionAnimated = self.PositionAnimated;
            _activateObjectPhysikAfterAnimation = self.ActivateGravityAtEnd;
            _rotationAnimated = Quaternion.Euler(self.RotationAnimated);
            _onedirectionAnimation = self.OnedirectionAnimation;
            _animationType = self.AnimationType;
            _keyType = self.KeyType;
            _animationDurationInFrames = self.AnimationDurationInFrames;
        }


        private Vector3 StepsPerFrame(Vector3 a, Vector3 b, int frames) {
            var result = new Vector3();
            if(a.x < 0 && b.x > 0) {
                a.x = a.x * -1;
                result.x = b.x + a.x;
            }

            if(a.x > 0 && b.x < 0) {
                result.x = b.x - a.x;
            }

            if(a.x > 0 && b.x > 0) {
                result.x = b.x - a.x;
            }

            if(a.x < 0 && b.x < 0) {
                a.x = a.x * -1;
                result.x = b.x + a.x;
            }

            if(a.y < 0 && b.y > 0) {
                a.y = a.y * -1;
                result.y = b.y + a.y;
            }

            if(a.y > 0 && b.y < 0) {
                result.y = b.y - a.y;
            }

            if(a.y > 0 && b.y > 0) {
                result.y = b.y - a.y;
            }

            if(a.y < 0 && b.y < 0) {
                a.y = a.y * -1;
                result.y = b.y + a.y;
            }

            if(a.z < 0 && b.z > 0) {
                a.z = a.z * -1;
                result.z = b.z + a.z;
            }

            if(a.z > 0 && b.z < 0) {
                result.z = b.z - a.z;
            }

            if(a.z > 0 && b.z > 0) {
                result.z = b.z - a.z;
            }

            if(a.z < 0 && b.z < 0) {
                a.z = a.z * -1;
                result.z = b.z + a.z;
            }
            
            return result / frames;
        }

        private void CalculateRotationSteps(Quaternion aQuaternion, Quaternion bQuaternion, int frames) {
            var a = Quaternion.Angle(aQuaternion, bQuaternion);
            a = a / frames;
            _rotationSteps = a;
        }

//End of Calculations

//Getter and Setter for Animations

        public bool Open {
            get {return _open;}
            set {_open = value;}
        }

        public bool GhostDrivenAnimationActive {
            get {return _ghostDrivenAnimationActive;}
            set {_ghostDrivenAnimationActive = value;}
        }

        public bool ParentAnimationDone {
            get {return _parentAnimationDone;}
            set {_parentAnimationDone = value;}
        }

//End of Getter and Setter for Animations
    }
}
