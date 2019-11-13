using System;
using JetBrains.Annotations;
using UnityEngine;

public class Key : Pickupable
{
    private Rigidbody _rb;
    private bool _initalCanBeInteractedWith;
    private Vector3 _initialPos;
    private Quaternion _initialRot;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _initalCanBeInteractedWith = CanBeInteractedWith;
        _initialPos = transform.localPosition;
        _initialRot = transform.localRotation;
    }

    protected override void OnPickup()
    {
        EventHandler.OnKeyPickUp(this);
        Debug.Log("Awe");
    }

    public void Freeze()
    {
        CanBeInteractedWith = false;
        if(_rb != null)
            _rb.isKinematic = true;
    }

    public void Unfreeze()
    {
        CanBeInteractedWith = true;
        if(_rb != null)
            _rb.isKinematic = false;
    }

    protected override void ResetObject()
    {
        CanBeInteractedWith = _initalCanBeInteractedWith;
        gameObject.SetActive(true);
        if (_rb != null)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }

        transform.localPosition = _initialPos;
        transform.localRotation = _initialRot;

    }
}