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

    protected override void OnPickup()
    {
        EventHandler.OnKeyPickUp(this);
    }

    public void Freeze()
    {
        CanBeInteractedWith = false;
        allowPickup = false;
        _rb.isKinematic = true;
    }

    public void Unfreeze()
    {
        CanBeInteractedWith = true;
        allowPickup = true;
        _rb.isKinematic = false;
    }

    protected override void ResetObject()
    {
        gameObject.SetActive(true);
    }
}