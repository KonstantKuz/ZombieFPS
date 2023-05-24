using UnityEngine;

namespace App.RagdollDismembermentSystem.Data.Snapshots
{
    public class CharacterJointSnapshot
    {
        private Rigidbody _connectedBody;
        private Vector3 _anchor;
        private Vector3 _axis;
        private bool _autoConfigureConnectedAnchor;
        private Vector3 _connectedAnchor;
        private Vector3 _swingAxis;
        private SoftJointLimit _lowTwistLimit;
        private SoftJointLimit _highTwistLimit;
        private SoftJointLimit _swing1Limit;
        private SoftJointLimit _swing2Limit;
        private float _breakForce;
        private float _breakTorque;
        private bool _enableCollision;
        private bool _enableProjection;
        private bool _enablePreprocessing;

        public CharacterJointSnapshot CreateSnapshot(CharacterJoint characterJoint)
        {
            _connectedBody = characterJoint.connectedBody;
            _anchor = characterJoint.anchor;
            _axis = characterJoint.axis;
            _autoConfigureConnectedAnchor = characterJoint.autoConfigureConnectedAnchor;
            _connectedAnchor = characterJoint.connectedAnchor;
            _swingAxis = characterJoint.swingAxis;
            _lowTwistLimit = characterJoint.lowTwistLimit;
            _highTwistLimit = characterJoint.highTwistLimit;
            _swing1Limit = characterJoint.swing1Limit;
            _swing2Limit = characterJoint.swing2Limit;
            _breakForce = characterJoint.breakForce;
            _breakTorque = characterJoint.breakTorque;
            _enableCollision = characterJoint.enableCollision;
            _enableProjection = characterJoint.enableProjection;
            _enablePreprocessing = characterJoint.enablePreprocessing;
            return this;
        }
        
        public void ApplySnapshot(CharacterJoint characterJoint)
        {
            characterJoint.connectedBody = _connectedBody;
            characterJoint.anchor = _anchor;
            characterJoint.axis = _axis;
            characterJoint.autoConfigureConnectedAnchor = _autoConfigureConnectedAnchor;
            characterJoint.connectedAnchor = _connectedAnchor;
            characterJoint.swingAxis = _swingAxis;
            characterJoint.lowTwistLimit = _lowTwistLimit;
            characterJoint.highTwistLimit = _highTwistLimit;
            characterJoint.swing1Limit = _swing1Limit;
            characterJoint.swing2Limit = _swing2Limit;
            characterJoint.breakForce = _breakForce;
            characterJoint.breakTorque = _breakTorque;
            characterJoint.enableCollision = _enableCollision;
            characterJoint.enableProjection = _enableProjection;
            characterJoint.enablePreprocessing = _enablePreprocessing;
        }
    }
}