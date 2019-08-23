using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    [SerializeField, Range(0.01f, 1f)] private float lerpSpeed = 0.3f;
    [SerializeField] private MainCamera _mainCamera;

    private float _moveDirection;
    private Animator _animator;
    private static readonly int Speed = Animator.StringToHash("Speed");

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (_mainCamera.Move)
        {
            _moveDirection = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
            _animator.SetFloat(Speed, _moveDirection);
        }
    }

    private void FixedUpdate()
    {
        if (_moveDirection > Mathf.Epsilon && _mainCamera.Move)
        {
            transform.position += speed * Time.deltaTime * transform.forward;
            transform.rotation = Quaternion.Lerp(transform.rotation, _mainCamera.FlatDirection, lerpSpeed);
        }
    }
}