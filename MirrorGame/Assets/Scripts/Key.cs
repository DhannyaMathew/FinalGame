using System;
using JetBrains.Annotations;
using UnityEngine;

public class Key : Pickupable
{
    private Rigidbody _rb;
    private bool _initalCanBeInteractedWith;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _initalCanBeInteractedWith = CanBeInteractedWith;
    }

    protected override void OnPickup()
    {
        EventHandler.OnKeyPickUp(this);
        Debug.Log("Awe");
    }

    public void Freeze()
    {
        CanBeInteractedWith = false;
        _rb.isKinematic = true;
    }

    public void Unfreeze()
    {
        CanBeInteractedWith = true;
        _rb.isKinematic = false;
    }

    public void MakeInteractable()
    {
        CanBeInteractedWith = true;
    }

    protected override void ResetObject()
    {
        CanBeInteractedWith = _initalCanBeInteractedWith;
        gameObject.SetActive(true);
    }
}