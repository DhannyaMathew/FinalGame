using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private Material Up;
    [SerializeField] private Material Down;

    [SerializeField] private PressurePadObject[] pressurePadObjects;
    private bool _isPressed;
    private Vector3 _initialPostion;
    private AudioSource _as;


    private void Start()
    {
        _initialPostion = transform.localPosition;
        _as = GetComponent<AudioSource>();
    }

    // Update is called once per frame

    public void PressDown()
    {
        _as.pitch = 1f;
        foreach (var o in pressurePadObjects)
        {
            o.OnPressurePadDown();
        }

        _as.Play();
        _isPressed = true;
        GetComponent<Renderer>().material = Down;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (Water.canBePressed)
        {
            PressDown();
        }
    }

    public void Unpress()
    {
        _as.pitch = 0.6f;
        _as.Play();
        foreach (var o in pressurePadObjects)
        {
            o.OnPressurePadUp();
        }

        GetComponent<Renderer>().material = Up;
        _isPressed = false;
    }

    private void OnCollisionExit(Collision other)
    {
        if (Water.canBePressed)
        {
            Unpress();
        }
    }
}