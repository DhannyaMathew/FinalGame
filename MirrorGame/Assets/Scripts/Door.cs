using System;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private float openSpeed;
    [SerializeField] private float openAngle;
    [SerializeField] private bool locked;
    [SerializeField] private bool open;

    private Door _connectedDoor;
    private bool _isLinked;
    private Transform _hinge;
    private Quaternion _targetRotation = Quaternion.identity;

    private Portal _portal;
    
    // Start is called before the first frame update
    void Start()
    {
        _hinge = transform.GetChild(0);
    }


    // Update is called once per frame
    void Update()
    {
        if (open)
        {
            _targetRotation = Quaternion.Euler(0, -openAngle, 0);
        }
        else
        {
            _targetRotation = Quaternion.Euler(0, 0, 0);
        }

        _hinge.localRotation = Quaternion.Lerp(_hinge.localRotation, _targetRotation, Time.deltaTime * openSpeed);
        
        if (_isLinked)
        {
            if (Math.Abs(_hinge.localRotation.eulerAngles.y) > 0.01f)
            {
                _portal.gameObject.SetActive(true);
                _portal.UpdatePortalCamera(GameManager.MainCamera.Camera);
            }
            else
            {
                _portal.gameObject.SetActive(false);
            }
        }

    }

    public void Lock()
    {
        locked = true;
        open = false;
        if (_isLinked)
            _connectedDoor.Lock();
    }

    public void Unlock()
    {
        locked = false;
        if (_isLinked)
            _connectedDoor.Unlock();
    }

    public override void OnInteract()
    {
        if (!locked)
        {
            open = !open;
            if (_isLinked)
                _connectedDoor.open = open;
        }
    }

    public void Link(Door other, bool connectingBack)
    {
        _connectedDoor = other;
        _isLinked = true;
        _portal = transform.GetChild(transform.childCount - 1).gameObject.GetComponent<Portal>();
        var otherPortal = other.transform.GetChild(other.transform.childCount - 1).gameObject.GetComponent<Portal>();
        _portal.gameObject.SetActive(true);
        if (!connectingBack)
        {
            otherPortal.gameObject.SetActive(true);
            otherPortal.transform.rotation *= Quaternion.Euler(0,180,0);
        }
        _portal.SetOtherPortal(otherPortal);
        
    }
}