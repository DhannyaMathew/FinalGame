using System;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Key : Pickupable
{
    [CanBeNull] private Joint _joint;
    private Rigidbody _rb;

    private void Awake()
    {
        _joint = GetComponent<Joint>();
        _rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        if (_isHeld)
            transform.localPosition = Vector3.zero;
    }

    public void ChildTo(Transform keyHold)
    {
        _rb.isKinematic = true;
        _rb.drag = 0;
        _rb.angularDrag = 0;
        transform.parent = keyHold;
        transform.localPosition = Vector3.zero;
        transform.rotation = keyHold.rotation;
    }

    protected override void OnPickup()
    {
        if (_joint != null) Destroy(_joint);
        EventHandler.OnKeyPickUp(this);
    }

    public void Freeze()
    {
        allowPickup = false;
        _rb.isKinematic = true;
    }

    public void Unfreeze()
    {
        allowPickup = true;
        _rb.isKinematic = false;
    }
}