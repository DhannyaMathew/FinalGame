using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    [SerializeField] private Vector2 minMaxDistance = new Vector2(3f, 7f);
    [SerializeField] private Vector2 minMaxDistortionStrength = new Vector2(0.001f, 0.1f);
    private Material _material;
    private static readonly int NormalStrength = Shader.PropertyToID("_normalStrength");

    private void Awake()
    {
        _material = GetComponentInChildren<Renderer>().material;
    }

    private void Update()
    {
        //y=(y1-y0)/(x1-x0)(x-x0)+y0
        var playerPosition = GameManager.PlayerTransform.position;
        var strength = (minMaxDistortionStrength.x - minMaxDistortionStrength.y) /
                       (minMaxDistance.x - minMaxDistance.y) *
                       ((playerPosition - transform.position).magnitude - minMaxDistance.x) +
                       minMaxDistortionStrength.x;
        strength = Mathf.Clamp(strength, minMaxDistortionStrength.x, minMaxDistortionStrength.y);
        _material.SetFloat(NormalStrength, strength);
    }

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