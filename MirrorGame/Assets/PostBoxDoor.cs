using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PostBoxDoor : MonoBehaviour
{
    private bool open;
    private float openAngle = -100f;
    private float angle;


    private void Start()
    {
        openAngle = Random.Range(-80f, -120f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(-90, angle, 0);
        angle = Mathf.Lerp(angle, open ? openAngle : 0, 0.15f);
    }
    
    public void Open()
    {
        open = true;
    }

    public void Toggle()
    {
        open = !open;
    }

    public void Close()
    {
        open = false;
    }
}