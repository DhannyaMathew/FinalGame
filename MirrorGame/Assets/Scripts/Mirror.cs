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
    private LinearMapping _offset;

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

    private AudioSource[] _soundsMirror;
    private AudioSource _soundReverse;

    private void Awake()
    {
        _soundsMirror = GetComponents<AudioSource>();
        _soundReverse = _soundsMirror[1];
        _material = GetComponentInChildren<Renderer>().material;
        _offset = new LinearMapping()
        {
            clamp =  true,
            minMaxIn = Vector2.up,
            minMaxOut = new Vector2(1f,3f)
        };
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
        if (CanBeInteractedWith)
        {
            Destroy(gameObject);
        }
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
            _soundReverse.Play();
            Level.Reflect();
            var mirrorTransform = transform;
            mirrorTransform.parent = null;
            Level.transform.parent = mirrorTransform;
            mirrorTransform.localScale = new Vector3(
                -1 * mirrorTransform.localScale.x, 1, 1);
            transform.localRotation = Quaternion.LookRotation(-1.8f * mirrorTransform.forward, mirrorTransform.up);
            
            
            other.transform.position += _offset.Evaluate(Mathf.Abs(Vector3.Dot(mirrorTransform.forward, Vector3.up)))* 3f * mirrorTransform.forward;
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

    private void OnDestroy()
    {
        Level.RemoveMirror(this);
        
    }
}