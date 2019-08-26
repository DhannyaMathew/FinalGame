using System;
using UnityEngine;

[Serializable]
public class ColourSwap
{
    public String shaderReference = "_BaseColor";
    public Color flippedColour;

    private Material _material;
    private Color _default;

    public void Set(Material material)
    {
        _material = material;
        _default = _material.GetColor(shaderReference);
    }

    public void Reflect(bool isReflected)
    {
        _material.SetColor(shaderReference, isReflected ? flippedColour : _default);
    }
}


public class Reflectable : MonoBehaviour
{
    [SerializeField] private ColourSwap[] colourSwaps;

    private Material _material;
    private bool _isReflected;

    private void Start()
    {
        _material = GetComponent<Renderer>().material;
        foreach (var colourSwap in colourSwaps)
        {
            colourSwap.Set(_material);
        }
    }

    public void Reflect()
    {
        _isReflected = !_isReflected;
        foreach (var colourSwap in colourSwaps)
        {
            colourSwap.Reflect(_isReflected);
        }
    }
}