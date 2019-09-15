using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private float openSpeed;
    [SerializeField] private float openAngle;
    [SerializeField] private bool locked;
    [SerializeField] private bool open;

    private int _direction;
    private Transform _hinge;
    private Quaternion _targetRotation = Quaternion.identity;

    private Quaternion _initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        _initialRotation = transform.localRotation;
        _hinge = transform.GetChild(0);
    }




    // Update is called once per frame
    void Update()
    {
        _hinge.localRotation = Quaternion.Lerp(_hinge.localRotation, _targetRotation, Time.deltaTime * openSpeed);
    }

    public void Lock()
    {
        locked = true;
        open = false;
    }

    public void Unlock()
    {
        locked = false;
    }

    public override void OnInteract()
    {
        if (!locked)
        {
            open = !open;
            if (open)
            {
                _targetRotation = _initialRotation * Quaternion.Euler(0, -openAngle, 0);
            }
            else
            {
                _targetRotation = _initialRotation;
            }
        }
    }
}