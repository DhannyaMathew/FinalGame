using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorEnter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        var level = GameObject.FindGameObjectWithTag("Level");
        transform.parent = null;
        level.transform.parent = transform;
        transform.localScale = new Vector3(
            -1 * transform.localScale.x, 1, 1);
        transform.localRotation = Quaternion.LookRotation(-1f * transform.forward);
        level.transform.parent = null;
        transform.parent = level.transform;
    }
}