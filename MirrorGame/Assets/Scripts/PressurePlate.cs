using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{

    [SerializeField] private PressurePadObject[] pressurePadObjects;
    private bool _isPressed;
    private Vector3 _initialPostion;
    

    private void Start()
    {
        _initialPostion = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPressed)
        {
            transform.localPosition  = Vector3.Lerp(transform.localPosition, _initialPostion - Vector3.up*0.1f, 0.05f);
        }
        else
        {
            
            transform.localPosition  = Vector3.Lerp(transform.localPosition, _initialPostion, 0.05f);
        } 
    }

    private void OnCollisionEnter(Collision other)
    {
        foreach (var o in pressurePadObjects)
            {
                o.OnPressurePadDown();
            }
            _isPressed = true;
        
    }
    private void OnCollisionExit(Collision other)
    {
        
            foreach (var o in pressurePadObjects)
            {
                o.OnPressurePadUp();
            }
            _isPressed = false;
    }
    
    
}
