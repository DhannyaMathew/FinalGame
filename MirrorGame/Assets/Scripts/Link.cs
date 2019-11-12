using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Link : MonoBehaviour
{
    private AnimationCurve _drag;
    private AnimationCurve _angularDrag;
    private LinearMapping _angularDragLM;
    private LinearMapping _dragLM;
    private Chain _chain;
    private Rigidbody _rb;
    private float _maxVelocity;
    private float _maxAngularVelocity;

    public bool AboveSoundThresh => _rb.velocity.sqrMagnitude > _chain.soundThresh * _chain.soundThresh;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _chain = GetComponentInParent<Chain>();
    }

    private void Update()
    {
        var v = Mathf.Clamp01(_rb.velocity.magnitude / _maxVelocity);
        var av = Mathf.Clamp01(_rb.velocity.magnitude / _maxVelocity);
        _rb.drag = _dragLM.Evaluate(Mathf.Clamp01(_drag.Evaluate(v)));
        _rb.angularDrag = _angularDragLM.Evaluate(Mathf.Clamp01(_angularDrag.Evaluate(av)));
        _rb.velocity = _rb.velocity.normalized * (v * _maxVelocity);
        _rb.angularVelocity = _rb.angularVelocity.normalized * (av * _maxAngularVelocity);
    }


    public void SetPhysics(AnimationCurve drag, AnimationCurve angularDrag, LinearMapping dragLM,
        LinearMapping angularDragLM, float maxVelocity, float maxAngularVelocity)
    {
        _dragLM = dragLM;
        _angularDragLM = angularDragLM;
        _drag = drag;
        _angularDrag = angularDrag;
        _maxVelocity = maxVelocity;
        _maxAngularVelocity = maxAngularVelocity;
    }
}