using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    public Transform Anchor { get; private set; }
    
    
    private void Start()
    {
        Anchor = transform.GetChild(0).GetChild(0);
    }
}
