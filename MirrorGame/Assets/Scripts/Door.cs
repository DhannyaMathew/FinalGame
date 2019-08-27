using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float openSpeed;
    [SerializeField] private float openAngle;
    [SerializeField] private bool locked;
    
    private float _currentAngle = 0;
    private Transform _hinge;
    
    // Start is called before the first frame update
    void Start()
    {
        _hinge = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
    }
}