using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Chain : MonoBehaviour
{
    [SerializeField] private float maxVelocity = 4;
    [SerializeField] private Vector2 minMaxDrag;
    [SerializeField] private AnimationCurve veloctiiyDrag;
    [SerializeField] private float maxAngularVelocity=4;
    [SerializeField] private Vector2 minMaxAngularDrag;
    [SerializeField] private AnimationCurve angularVeloctiiyAngularDrag;
    
    private void Start()
    {
        var dragLM = new LinearMapping{minMaxIn =  Vector2.up, minMaxOut = minMaxDrag};
        var angularDragLM = new LinearMapping{minMaxIn =  Vector2.up, minMaxOut = minMaxAngularDrag};
        foreach (var link in   GetComponentsInChildren<Link>())
        {
            
            link.SetPhysics(veloctiiyDrag, angularVeloctiiyAngularDrag, dragLM, angularDragLM, maxVelocity, maxAngularVelocity);   
        }
    }
    
}