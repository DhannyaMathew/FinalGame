using System;
using UnityEngine;
using UnityEngine.Events;


[Serializable] public class ReflectionAttribute
{
    [SerializeField] private ColourSwap[] colourSwaps;
    [SerializeField] private MapSwap[] mapSwaps;
    [SerializeField] private MaterialSwap[] materialSwaps;
    [SerializeField] private ReflectEvent reflectEvent;
    private bool _isReflected;

    internal void Set()
    {
        foreach (var colourSwap in colourSwaps)
        {
            colourSwap.Set();
        }

        foreach (var mapSwap in mapSwaps)
        {
            mapSwap.Set();
        }
        foreach (var materialSwap in materialSwaps)
        {
            materialSwap.Set();
        }

    }

    internal void Reflect()
    {
        _isReflected = !_isReflected;
        foreach (var colourSwap in colourSwaps)
        {
            colourSwap.Reflect(_isReflected);
        }

        foreach (var mapSwap in mapSwaps)
        {
            mapSwap.Reflect(_isReflected);
        }
        foreach (var materialSwap in materialSwaps)
        {
            materialSwap.Reflect(_isReflected);
        }
        reflectEvent.Reflect(_isReflected);
    }

    internal void OnDisable()
    {
        if(_isReflected)
            Reflect();
    }
}


[Serializable] public class ColourSwap
{
    [SerializeField] private Material material;
    [SerializeField] private string shaderReference = "_BaseColor";
    [SerializeField] private Color flippedColour;
    private Color _default;

    public void Set()
    {
        _default = material.GetColor(shaderReference);
    }

    public void Reflect(bool isReflected)
    {
        material.SetColor(shaderReference, isReflected ? flippedColour : _default);
    }
}


[Serializable] public class MapSwap
{
    [SerializeField] private Material material;
    [SerializeField] private string shaderReference;
    [SerializeField] private Texture flippedColour;

    private Texture _default;

    public void Set()
    {
        _default = material.GetTexture(shaderReference);
    }

    public void Reflect(bool isReflected)
    {
        material.SetTexture(shaderReference, isReflected ? flippedColour : _default);
    }
}


[Serializable] public class MaterialSwap
{
    [SerializeField] private Renderer renderer;
    [SerializeField] private Material material;

    private Material _default;

    public void Set()
    {
        _default = renderer.material;
    }

    public void Reflect(bool isReflected)
    {
        renderer.material =  isReflected ? material : _default;
    }
}


[Serializable] public class ReflectEvent
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


