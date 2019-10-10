﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PlayerManager
{
    internal class PlayerMove
    {
        private float _speed;
        private Vector3 _averageContactNormal;
        private bool _grounded;
        private bool _laddered;
        private bool _bottomContactConnected;
        private bool _topContactConnected;
        private float _ladderAngle;
        private readonly Rigidbody _rigidBody;
        private readonly CharacterSettings _settings;
        private float InputAngle => Vector3.SignedAngle(Vector3.forward, MoveDirection, Vector3.up);

        private float BodyAngle
        {
            get
            {
                /*if (_laddered && !_grounded)
                {
                    return _ladderAngle;
                }

                if (_grounded && _laddered)
                {
                    return MoveDirection.z > 0 ? _ladderAngle : _rigidBody.rotation.eulerAngles.y;
                }*/

                return _rigidBody.rotation.eulerAngles.y;
            }
        }

        private Quaternion ForwardRotation => Quaternion.AngleAxis(InputAngle + BodyAngle, _averageContactNormal);

        private Vector3 Forward => ForwardRotation * Quaternion.LookRotation(_averageContactNormal, Vector3.forward) *
                                   Vector3.up;

        internal float NormalizedSpeed => Speed * MoveDirection.magnitude / _settings.runSpeed;
        internal Vector3 MoveDirection { get; private set; }
        internal float Speed { get; private set; }

        internal bool HasInput => MoveDirection.sqrMagnitude > 0.01f * 0.01f;

        private bool OnWalkableGround => Vector3.Angle(_averageContactNormal, Vector3.up) < _settings.maxGroundAngle;
        private bool TallEnough => _bottomContactConnected && !_topContactConnected;

        internal PlayerMove(CharacterSettings settings, Rigidbody rigidBody)
        {
            _settings = settings;
            _speed = settings.walkSpeed;
            _rigidBody = rigidBody;
        }

        internal void Move(float cameraFlatAngle, IEnumerable<ContactPoint> collisions)
        {
            UpdateContactNormal(collisions);
            CalculateSpeed();
            Rotate(cameraFlatAngle);
            Move();
        }

        internal void GetInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _speed = _settings.runSpeed;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _speed = _settings.walkSpeed;
            }

            var t = Mathf.Clamp01(Vector3.Dot(Forward, Vector3.up));
            _speed = Mathf.Lerp(_speed, _settings.climbSpeed, t);
            MoveDirection =
                Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal") / 2f, 0, Input.GetAxis("Vertical")), 1f);
        }

        private void UpdateContactNormal(IEnumerable<ContactPoint> collisions)
        {
            _bottomContactConnected = _topContactConnected = _grounded = _laddered = false;
            var groundAverage = Vector3.zero;
            var ladderAverage = Vector3.zero;
            foreach (var contact in collisions)
            {
                var layer = contact.otherCollider.gameObject.layer;
                var angle = Vector3.Angle(contact.normal, Vector3.up);
                if (FindLadder(contact.otherCollider.transform, layer))
                {
                    ladderAverage += contact.normal;
                }

                if (FindGround(contact.point, angle, layer))
                {
                    groundAverage += contact.normal;
                }
            }

            groundAverage.Normalize();
            ladderAverage.Normalize();


            if (!_grounded && !_laddered)
            {
                _averageContactNormal = Vector3.up;
            }
            else if (_grounded && !_laddered)
            {
                _averageContactNormal = groundAverage;
            }
            else if (_laddered && !_grounded)
            {
                _averageContactNormal = ladderAverage;
            }
            else if (_grounded && _laddered)
            {
                _averageContactNormal = MoveDirection.z > 0 ? ladderAverage : groundAverage;
            }
        }

        private void CalculateSpeed()
        {
            Speed = Mathf.Lerp(Speed, _speed, Time.deltaTime * _settings.acceleration);
        }

        private void Rotate(float cameraFlatAngle)
        {
            var direction = Quaternion.Euler(0, cameraFlatAngle, 0);
            _rigidBody.rotation =
                Quaternion.LerpUnclamped(_rigidBody.rotation, direction, Time.deltaTime * _settings.rotateSpeed);
        }

        private void Move()
        {
            if ((_grounded && (OnWalkableGround || TallEnough)) || _laddered)
                _rigidBody.velocity = MoveDirection.magnitude * Speed * Forward;
            else
            {
                _rigidBody.velocity += Physics.gravity * Time.deltaTime;
            }
        }

        private static bool CheckLayer(LayerMask input, LayerMask compare)
        {
            var val = ((1 << input.value) & compare.value) != 0;
            return val;
        }

        private bool FindLadder(Transform potentialLadder, LayerMask input)
        {
            if (CheckLayer(input, _settings.ladder))
            {
                var ab = _rigidBody.position - potentialLadder.position;
                var c = Vector3.Dot(ab, potentialLadder.right);
                var d = Mathf.Abs(c);
                var a = d < 0.3f;
                if (_settings.debug)
                {
                    Debug.DrawLine(potentialLadder.position, potentialLadder.position + potentialLadder.right * c,
                        a ? Color.green : Color.red);
                }

                var direction = Mathf.Sign(Vector3.Dot(ab, potentialLadder.forward));
                _ladderAngle =
                    -Vector3.SignedAngle(Vector3.forward, potentialLadder.forward, Vector3.up);
                _laddered = _laddered || a;
                return a;
            }

            return false;
        }

        private bool FindGround(Vector3 point, float angle, LayerMask input)
        {
            if (CheckLayer(input, _settings.ground))
            {
                var diff = point.y - _rigidBody.position.y;
                _bottomContactConnected = _bottomContactConnected || diff <
                                          _settings.height * _settings.bottomDetector;
                _topContactConnected = _topContactConnected || diff >
                                       _settings.height * _settings.topDetector;
                var a = angle < _settings.maxGroundAngle;
                _grounded = _grounded || a;
                return a;
            }

            return false;
        }

        internal void DrawDebug()
        {
#if UNITY_EDITOR
            var pos = _rigidBody.position;
            Gizmos.color = _bottomContactConnected ? Color.green : Color.red;
            Gizmos.DrawSphere(pos + _settings.height * _settings.bottomDetector * Vector3.up, 0.05f);
            Gizmos.color = _topContactConnected ? Color.green : Color.red;
            Gizmos.DrawSphere(pos + _settings.height * _settings.topDetector * Vector3.up, 0.05f);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(pos, pos + Forward);
            Handles.color = Color.blue;
            Handles.DrawWireArc(pos, _averageContactNormal,
                Vector3.forward, InputAngle + BodyAngle, 0.6f);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(pos, pos + _averageContactNormal);
#endif
        }
    }
}