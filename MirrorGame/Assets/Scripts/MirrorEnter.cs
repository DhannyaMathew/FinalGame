using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorEnter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        var level = GameObject.FindGameObjectWithTag("Level");
        foreach (var reflectable in level.GetComponentsInChildren<Reflectable>())
        {
            reflectable.Reflect();
        }
        
        var mirrorTransform = transform;
        mirrorTransform.parent = null;
        level.transform.parent = mirrorTransform;
        mirrorTransform.localScale = new Vector3(
            -1 * mirrorTransform.localScale.x, 1, 1);
        transform.localRotation = Quaternion.LookRotation(-1f * mirrorTransform.forward);
        level.transform.parent = null;
        transform.parent = level.transform;
    }
}