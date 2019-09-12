using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;

[Serializable]
public class LinearMapping
{
    public Vector2 minMaxIn;
    public Vector2 minMaxOut;
    public bool clamp = true;

    public float Evaluate(float x)
    {
        var raw = (minMaxOut.x - minMaxOut.y) / (minMaxIn.x - minMaxIn.y) * (x - minMaxIn.x) + minMaxOut.x;
        return clamp ? Mathf.Clamp(raw, minMaxOut.x, minMaxOut.y) : raw;
    }
}

public class Mirror : MonoBehaviour
{
    [SerializeField] private LinearMapping distortion;
    [SerializeField] private LinearMapping fov;


    private Material _material;
    private HDProbe _prp;
    private static readonly int NormalStrength = Shader.PropertyToID("_normalStrength");

    private void Awake()
    {
        _material = GetComponentInChildren<Renderer>().material;
        _prp = GetComponentInChildren<HDProbe>();
        Debug.Log(_prp);
    }

    private void Update()
    {
        //y=(y1-y0)/(x1-x0)(x-x0)+y0
        var playerPosition = GameManager.PlayerTransform.position;
        var dist = (playerPosition - transform.position).magnitude;
        var strength = distortion.Evaluate(dist);
        var fov = this.fov.Evaluate(dist);
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
        other.transform.position += mirrorTransform.forward*2f;
        level.transform.parent = null;
        transform.parent = level.transform;
    }
}