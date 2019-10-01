using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientLighting : MonoBehaviour
{
    [SerializeField] private bool enabled;
    [SerializeField] private Color color;
    [SerializeField] private float intensity;
    [SerializeField] private Light[] _lights;

    // Start is called before the first frame update
    void Start()
    {
        Set();
    }

    public void Set()
    {
        foreach (var light1 in _lights)
        {
            light1.gameObject.SetActive(enabled);
            light1.color = color;
            light1.intensity = intensity;
        }
    }


    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
    }

    public void SetValues(Color color, float intensity, bool enabled)
    {
        this.intensity = intensity;
        this.enabled = enabled;
        this.color = color;
        Set();
    }
}