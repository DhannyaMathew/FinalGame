using System;

using UnityEngine;

public class Reflectable : MonoBehaviour
{

    [SerializeField] private String ColourReference;
    [SerializeField] private Color reflectedColour;
    private Color _default;
    private Material _material;
    private bool _isReflected;

    private void Start()
    {
        _material = GetComponent<Renderer>().material;
        _default = _material.GetColor(ColourReference);
    }

    public void Reflect()
    {
        _isReflected = !_isReflected;
        _material.SetColor(ColourReference, _isReflected?reflectedColour:_default);
    }
}
