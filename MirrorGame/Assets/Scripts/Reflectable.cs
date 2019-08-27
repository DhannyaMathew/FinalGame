using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class ColourSwap
{
    [SerializeField] private string shaderReference = "_BaseColor";
    [SerializeField] private Color flippedColour;

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

[Serializable]
public class ReflectEvent
{
    [SerializeField] private UnityEvent onReflect;
    [SerializeField] private UnityEvent onRevert;

    public void Reflect(bool isReflected)
    {
        if (isReflected)
            onReflect.Invoke();
        else
            onRevert.Invoke();
    }
}


public class Reflectable : MonoBehaviour
{
    [SerializeField] private ColourSwap[] colourSwaps;
    [SerializeField] private ReflectEvent reflectEvent;
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
        reflectEvent.Reflect(_isReflected);
    }
}