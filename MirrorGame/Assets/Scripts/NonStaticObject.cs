using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonStaticObject : LevelObject
{
    private Vector3 _initialPos;
    private Quaternion _initialRot;
    protected Rigidbody _rigidbody;

    protected override void Start()
    {
        base.Start();
        _rigidbody = GetComponent<Rigidbody>();
        _initialPos = transform.localPosition;
        _initialRot = transform.localRotation;
    }

    protected override void ResetObject()
    {
        transform.localPosition = _initialPos;
        transform.localRotation = _initialRot;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
}