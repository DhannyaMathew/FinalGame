using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairPressure : PressurePadObject
{
    [SerializeField] private float dist;
    private bool _isDown;
    private Vector3 _initialPos;
    private AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _initialPos = transform.localPosition;
    }

    private void Update()
    {
        if (_isDown)
        {
      

            transform.localPosition =
                Vector3.Lerp(transform.localPosition, _initialPos + transform.forward * dist, 0.025f);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _initialPos, 0.025f);
        }
    }

    public override void OnPressurePadDown()
    {
        _isDown = true;
    }

    public override void OnPressurePadUp()
    {
        _isDown = false;
    }
}