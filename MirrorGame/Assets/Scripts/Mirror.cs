using System;
using UnityEngine;


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

public class Mirror : Interactable
{
    [SerializeField] private LinearMapping distortion;
    [SerializeField] private float dissolveSpeed = 1f;

    private Material _material;
    private static readonly int NormalStrength = Shader.PropertyToID("_normalStrength");
    private float _dissolveAmount;

    public float DissolveAmount
    {
        get => _dissolveAmount;
        set
        {
            _dissolveAmount = Mathf.Clamp01(value);
            _material.SetFloat(DissolveAmountID, _dissolveAmount);
        }
    }

    private int _direction = 0;
    private static readonly int DissolveAmountID = Shader.PropertyToID("_dissolveAmount");

    private void Awake()
    {
        _material = GetComponentInChildren<Renderer>().material;
    }

    protected override void Start()
    {
        base.Start();
        CanBeInteractedWith = GameManager.CanPickupMirrors;
    }

    public void SetLevel(Level level)
    {
        if (Level != null)
        {
            Level.RemoveMirror(this);
        }

        Level = level;
        Level.AddMirror(this);
        transform.parent = level.transform;
    }

    public void FadeIn()
    {
        _direction = -1;
    }

    public void FadeOut()
    {
        _direction = 1;
    }

    protected override void ResetObject()
    {
    }

    private void Update()
    {
        //y=(y1-y0)/(x1-x0)(x-x0)+y0
        var playerPosition = GameManager.Player.transform.position;
        var dist = (playerPosition - transform.position).magnitude;
        var strength = distortion.Evaluate(dist);
        _material.SetFloat(NormalStrength, strength);
        if (_direction != 0)
        {
            var val = DissolveAmount + dissolveSpeed * _direction * Time.deltaTime;
            if (val > 1)
            {
                Level.RemoveMirror(this);
                Destroy(gameObject);
                return;
            }

            if (val < 0)
            {
                _direction = 0;
            }

            DissolveAmount = val;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Level.Reflect();
            var mirrorTransform = transform;
            mirrorTransform.parent = null;
            Level.transform.parent = mirrorTransform;
            mirrorTransform.localScale = new Vector3(
                -1 * mirrorTransform.localScale.x, 1, 1);
            transform.localRotation = Quaternion.LookRotation(-1.8f * mirrorTransform.forward, mirrorTransform.up);
            other.transform.position += mirrorTransform.forward * 2f;
            Level.transform.parent = null;
            transform.parent = Level.transform;
            Level.ResetMirrors();
            if (EventHandler.OnMirrorWalkThrough != null)
                EventHandler.OnMirrorWalkThrough(this);
        }
    }

    protected override void OnInteract()
    {
        DissolveAmount = 0f;
        FadeOut();
        if (EventHandler.OnMirrorAbsorb != null)
            EventHandler.OnMirrorAbsorb();
    }
}