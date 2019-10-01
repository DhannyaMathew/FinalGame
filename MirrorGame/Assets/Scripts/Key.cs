using System;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Key : Interactable
{
    private bool _isHeld;
    [SerializeField] private bool allowPickup = true;
    [CanBeNull] private Joint _joint;
    private Rigidbody _rb;

    private void Awake()
    {
        _joint = GetComponent<Joint>();
        _rb = GetComponent<Rigidbody>();
    }

    protected override void OnInteract()
    {
        if (allowPickup)
        {
            if (!_isHeld)
            {
                if (_joint != null) Destroy(_joint);
                _isHeld = true;
                EventHandler.OnKeyPickUp(this);
            }
        }
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

    public void Unchild()
    {
        transform.parent = GameManager.CurrentLevel.transform;
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