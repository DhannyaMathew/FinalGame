using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Chain : MonoBehaviour
{
    private Link[] _links;

    private Link Root => _links.Length > 0 ? _links[0] : null;
    private int Length => _links.Length;

    private void Awake()
    {
        _links = GetComponentsInChildren<Link>();
    }

    private void Update()
    {
        for (int i = Length - 1; i >= 1; i--)
        {
            _links[i].transform.position = _links[i-1].Anchor.position;
        }
    }
}