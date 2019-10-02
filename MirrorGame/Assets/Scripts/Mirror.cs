using System;
using UnityEngine;


[Serializable]public class LinearMapping
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

    private Material _material;
    private Level _level;
    private static readonly int NormalStrength = Shader.PropertyToID("_normalStrength");

    private AudioSource[] _soundsMirror;
    private AudioSource _soundReverse;

    private void Awake()
    {
        _soundsMirror = GetComponents<AudioSource>();
        _soundReverse = _soundsMirror[1];
        _material = GetComponentInChildren<Renderer>().material;
        _level = GetComponentInParent<Level>();
    }

    private void Update()
    {
        //y=(y1-y0)/(x1-x0)(x-x0)+y0
        var playerPosition = GameManager.Player.transform.position;
        var dist = (playerPosition - transform.position).magnitude;
        var strength = distortion.Evaluate(dist);
        _material.SetFloat(NormalStrength, strength);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _soundReverse.Play();
            _level.Reflect();
            var mirrorTransform = transform;
            mirrorTransform.parent = null;
            _level.transform.parent = mirrorTransform;
            mirrorTransform.localScale = new Vector3(
                -1 * mirrorTransform.localScale.x, 1, 1);
            transform.localRotation = Quaternion.LookRotation(-1.8f * mirrorTransform.forward, mirrorTransform.up);
            other.transform.position += mirrorTransform.forward*2f;
            _level.transform.parent = null;
            transform.parent = _level.transform;
            _level.ResetMirrors();
            if(EventHandler.OnMirrorWalkThrough != null)
                EventHandler.OnMirrorWalkThrough(this);
        }
    }
}