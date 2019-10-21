using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolePressure : PressurePadObject
{
    [SerializeField] private float rotation;
    [SerializeField] private float speed = 10;
    private bool _isDown;
    private Quaternion _initialRotation;
    private float _rot = 0;

    private void Start()
    {
        _initialRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isDown)
        {
            if (Mathf.Sign(rotation) * _rot > 0)
            {
                _rot -= Mathf.Sign(rotation) * speed * Time.deltaTime;
            }
            else
            {
                _rot = 0;
            }
        }
        else
        {
            if (_rot > rotation)
            {
                _rot += Mathf.Sign(rotation) * speed * Time.deltaTime;
            }
            else
            {
                _rot = rotation;
            }
        }

        transform.localRotation = _initialRotation * Quaternion.Euler(0, _rot, 0);
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