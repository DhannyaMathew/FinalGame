using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairPressure : PressurePadObject
{
    [SerializeField] private float dist;
    private bool _isDown;
    private Vector3 _initialPos;

    private void Start()
    {
        _initialPos = transform.localPosition;
    }

    private void Update()
    {
        if (_isDown)
        {
            
            transform.localPosition = Vector3.Lerp(transform.localPosition, _initialPos+transform.forward*dist, 0.025f);
        }
        else
        {
            
            transform.localPosition = Vector3.Lerp(transform.localPosition, _initialPos+transform.forward, 0.025f);
        }
    }

    public override void OnPressurePadDown()
    {
        Debug.Log("Awe1");
        _isDown = true;
    }

    public override void OnPressurePadUp()
    {
        Debug.Log("Awe2");
        _isDown = false;
    }
}
