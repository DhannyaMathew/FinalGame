using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 4.5f;
    [SerializeField, Range(0.01f, 1f)] private float rotateLerpSpeed = 0.085f;
    [SerializeField, Range(0.01f, 1f)] private float accelerationFactor = 0.1f;
    [SerializeField] private float height;
    [SerializeField] private float heightPadding;
    [SerializeField] private bool debug;
    [SerializeField] private float maxGroundAngle;
    [SerializeField] private LayerMask ground;

    private float _speed;
    private float _actualSpeed;
    private bool _isRunning;
    private Vector3 _moveDirection;
    private Animator _animator;
    private bool _grounded;
    private RaycastHit _hitInfo;
    private float _angle;

    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int YDir = Animator.StringToHash("yDir");
    private static readonly int XDir = Animator.StringToHash("xDir");

    public Vector3 Forward { get; private set; }
    public float GroundAngle { get; private set; }

    private void Awake()
    {
        _speed = walkSpeed;
        _animator = GetComponentInChildren<Animator>();
    }

    public void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed = runSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = walkSpeed;
        }

        _moveDirection =
            Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal") / 2f, 0, Input.GetAxis("Vertical")), 1f);
    }

    public void Move(float cameraFlatAngle)
    {
        GetInput();
        CalculateGroundAngle();
        CheckGround();
        ApplyGravity();
        DrawDebugLines();
        CalculateSpeed();
        UpdateAnimator();
        Rotate(cameraFlatAngle);
        if (_moveDirection.sqrMagnitude > 0.1f * 0.1f)
        {
            CalculateForward();
            Move();
        }
    }

    private void UpdateAnimator()
    {
        _animator.SetFloat(Speed, _actualSpeed * _moveDirection.magnitude / runSpeed);
        _animator.SetFloat(XDir, _moveDirection.x);
        _animator.SetFloat(YDir, _moveDirection.y);
    }

    private void Move()
    {
        if (GroundAngle > maxGroundAngle) return;
        transform.position += _actualSpeed * Time.deltaTime * Forward;
    }

    private void Rotate(float cameraFlatAngle)
    {
        var direction = Quaternion.Euler(0, cameraFlatAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotateLerpSpeed);
    }

    private void CalculateSpeed()
    {
        _actualSpeed = Mathf.Lerp(_actualSpeed, _speed, accelerationFactor);
    }

    private void DrawDebugLines()
    {
        if (debug)
        {
            Debug.DrawLine(transform.position + Vector3.up * height,
                transform.position + Vector3.up * height + height * Forward, Color.blue);
            Debug.DrawLine(transform.position + Vector3.up * height, transform.position - Vector3.up * heightPadding,
                Color.green);
            Debug.Log(GroundAngle);
        }
    }

    private void CheckGround()
    {
        if (Physics.Raycast(transform.position + Vector3.up * height, Vector3.down, out _hitInfo,
            height + heightPadding, ground))
        {
            var dh = Vector3.Distance(transform.position + Vector3.up * height, _hitInfo.point) - height;
            if (dh < 0)
            {
                transform.position -= Vector3.up * dh;
            }

            _grounded = true;
        }
        else
        {
            _grounded = false;
        }
    }

    private void ApplyGravity()
    {
        if (!_grounded)
        {
            transform.position += Physics.gravity * Time.deltaTime;
        }
    }

    private void CalculateGroundAngle()
    {
        if (!_grounded)
            GroundAngle = 0;
        else
            GroundAngle = Vector3.Angle(_hitInfo.normal, Quaternion.LookRotation(_moveDirection)*transform.forward)-90;
    }

    private void CalculateForward()
    {
        if (!_grounded)
            Forward = transform.forward;
        else
            Forward = Vector3.Cross(_hitInfo.normal, Quaternion.LookRotation(_moveDirection) * -transform.right);
    }
}