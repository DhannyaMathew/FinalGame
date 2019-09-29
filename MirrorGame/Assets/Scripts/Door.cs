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
    private bool _isEntrance;
    private Portal _portal;
    public bool IsEntrance => _isEntrance;
    public Transform KeyHole { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        _hinge = transform.GetChild(0);
        KeyHole = GameObject.FindWithTag("KeyHole").transform;
    }


    // Update is called once per frame
    private void Update()
    {
        _targetRotation = open ? Quaternion.Euler(0, -openAngle, 0) : Quaternion.Euler(0, 0, 0);
        _hinge.localRotation = Quaternion.Lerp(_hinge.localRotation, _targetRotation, Time.deltaTime * openSpeed);
        UpdatePortals(GameManager.MainCamera.Camera);
    }

    private void UpdatePortals(Camera camera)
    {
        if (_isLinked)
        {
            if (IsOpen() && InRange(camera) && InFrontOf(camera))
            {
                _portal.gameObject.SetActive(true);
                _portal.UpdatePortalCamera(camera);
            }
            else
            {
                _portal.gameObject.SetActive(false);
            }
        }
    }

    public void Link(Door other, bool connectingBack)
    {
        _isEntrance = connectingBack;
        _connectedDoor = other;
        _isLinked = true;
        _portal = transform.GetChild(transform.childCount - 1).gameObject.GetComponent<Portal>();
        var otherPortal = other.transform.GetChild(other.transform.childCount - 1).gameObject.GetComponent<Portal>();
        _portal.gameObject.SetActive(true);
        if (!connectingBack)
        {
            other.openAngle = openAngle;
            other.openSpeed = openSpeed;
            other.locked = locked;
            other.open = open;
            otherPortal.gameObject.SetActive(true);
            otherPortal.transform.rotation *= Quaternion.Euler(0, 180, 0);
        }

        _portal.SetOtherPortal(otherPortal);
    }

    private bool IsOpen()
    {
        return Math.Abs(_hinge.localRotation.eulerAngles.y) > 0.01f;
    }

    private bool InRange(Camera camera)
    {
        return Vector3.Distance(camera.transform.position, transform.position) < 50;
    }

    private bool InFrontOf(Camera camera)
    {
        return Vector3.Dot(camera.transform.forward, _portal.transform.forward) < 0;
    }

    public void Lock()
    {
        locked = true;
        open = false;
        if (_isLinked)
        {
            _connectedDoor.locked = true;
            _connectedDoor.open = false;
        }
    }

    public void Unlock()
    {
        locked = false;
        if (_isLinked)
            _connectedDoor.locked = false;
    }

    public void Close(bool andLock)
    {
        open = false;
        locked = andLock;
        if (_isLinked)
        {
            _connectedDoor.open = false;
            _connectedDoor.locked = andLock;
        }
    }

    private void TryOpen()
    {
        if (!locked)
        {
            open = !open;
            if (_isLinked)
                _connectedDoor.open = open;
        }
    }

    protected override void OnInteract()
    {
        if(EventHandler.OnDoorInteract != null)
            EventHandler.OnDoorInteract(this);
        TryOpen();
    }

    public void ForceOpen()
    {
        open = true;
        locked = false;
        if (_isLinked)
        {
            _connectedDoor.open = true;
            _connectedDoor.locked = false;
        }
    }
}