using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private Door exit;
    [SerializeField] private float openSpeed;
    [SerializeField] private float openAngle;
    [SerializeField] private bool locked;
    [SerializeField] private bool open;
    private int _direction;
    private Transform _hinge;
    private Quaternion _targetRotation = Quaternion.identity;
    private Quaternion _initialRotation;
    private Portal _portal;


    private void Awake()
    {
        _portal = GetComponent<Portal>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        _initialRotation = transform.rotation;
        _hinge = transform.GetChild(0);
        _portal.SetExitPortal(exit._portal);
    }

    
    // Update is called once per frame
    void Update()
    {
        if (open)
        {
            _targetRotation = _initialRotation * Quaternion.Euler(0, -openAngle, 0);
        }
        else
        {
            _targetRotation = _initialRotation;
        }
        _hinge.rotation = Quaternion.Lerp(_hinge.rotation, _targetRotation, Time.deltaTime * openSpeed);
    }

    public void Lock()
    {
        locked = true;
        open = false;
        exit.Lock();
    }

    public void Unlock()
    {
        locked = false;
        exit.Unlock();
    }

    public override void OnInteract()
    {
        if (!locked)
        {
            open = !open;
            exit.open = open;
        }
    }
}