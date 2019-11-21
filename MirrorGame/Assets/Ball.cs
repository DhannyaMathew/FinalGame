using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : NonStaticObject
{

    private Animator _animator;

    protected override void Start()
    {
        base.Start();
        _animator = GetComponentInChildren<Animator>();
    }

    public void Reflect()
    {
        _animator.enabled = !_animator.enabled;
        _rigidbody.constraints ^= RigidbodyConstraints.FreezePositionY;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            _animator.enabled = true;
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        }
    }
 

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            _animator.enabled = false;
            _rigidbody.constraints = 0;
        }
    }
}