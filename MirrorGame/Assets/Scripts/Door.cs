using System;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private float openSpeed;
    [SerializeField] private float openAngle;
    [SerializeField] private bool locked;
    [SerializeField] internal bool open;

    private bool _initialOpen;
    private bool _initialLocked;


    private Door _connectedDoor;
    private bool _isLinked;
    private Transform _hinge;
    private Quaternion _targetRotation = Quaternion.identity;
    private Portal _portal;
    private bool _triggerLockAndClose;
    public bool IsEntrance { get; private set; }
    public Transform KeyHole { get; private set; }
    public bool IsLocked => locked;

    private AudioSource[] _soundsDoor;
    private AudioSource _soundOpening;
    private AudioSource _soundClosing;
    private AudioSource _soundLocked;



    protected override void Start()
    {
        base.Start();
        _hinge = transform.GetChild(0);
        KeyHole = GameObject.FindWithTag("KeyHole").transform;
        
        _soundsDoor = GetComponents<AudioSource>();
        _soundOpening = _soundsDoor[0];
        _soundClosing = _soundsDoor[1];
        _soundLocked = _soundsDoor[2];
        _initialOpen = open;
        _initialLocked = locked;
      
    }

    private void Update()
    {
        _targetRotation = open ? Quaternion.Euler(0, -openAngle, 0) : Quaternion.Euler(0, 0, 0);
        _hinge.localRotation = Quaternion.Lerp(_hinge.localRotation, _targetRotation, Time.deltaTime * openSpeed);
        if ((GameManager.Player.transform.position - transform.position).sqrMagnitude>  5 * 5 && Level == GameManager.CurrentLevel)
        {
            Close(false);
        }
        
        UpdatePortals(GameManager.MainCamera.Camera);
    }

    private void UpdatePortals(Camera camera)
    {
        if (_isLinked)
        {
            if (IsOpen() && InRange(camera) && InFrontOf(camera))
            {
                _portal.Camera.gameObject.SetActive(true);
                _portal.PortalTarget.SetActive(true);
                _portal.UpdatePortalCamera(camera);
            }
            else
            {
                _portal.PortalTarget.SetActive(false);
                _portal.Camera.gameObject.SetActive(false);
            }
        }
    }

    private bool IsOpen()
    {
        return Math.Abs(_hinge.localRotation.eulerAngles.y) > 0f;
    }

    public void Link(Door other, bool connectingBack)
    {
        IsEntrance = connectingBack;
        _connectedDoor = other;
        _isLinked = true;
        _portal = transform.GetChild(transform.childCount - 1).gameObject.GetComponent<Portal>();

        _portal.gameObject.SetActive(true);
        _connectedDoor.transform.localScale = transform.localScale;
        var otherPortal = other.transform.GetChild(other.transform.childCount - 1).gameObject.GetComponent<Portal>();
        EventHandler.OnMirrorWalkThrough += mirror =>
        {
            if (mirror.Level == Level)
            {
                _connectedDoor.transform.localScale = Vector3.Scale(new Vector3(Mathf.Sign(transform.lossyScale.x),1,1), _connectedDoor.transform.localScale);
            }
        };
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

    private bool InRange(Camera camera)
    {
        return Vector3.Distance(camera.transform.position, transform.position) < 50;
    }

    private bool InFrontOf(Camera camera)
    {
        return Vector3.Angle(camera.transform.forward, _portal.transform.forward) > -30;
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
        if (open)
        {
            _soundClosing.Play();
            open = false;
            locked = andLock;
            if (_isLinked)
            {
                _connectedDoor.open = false;
                _connectedDoor.locked = andLock;
            }
        }
    }

    private void TryOpen()
    {
        if (!locked)
        {

            if(!open)
                _soundOpening.Play();
            else
                _soundClosing.Play();
            open = !open;
            if (_isLinked)
                _connectedDoor.open = open;
        }
        else
        {
            _soundLocked.Play();
        }
    }
    

    protected override void OnInteract()
    {
        
        if (EventHandler.OnDoorInteract != null)
            EventHandler.OnDoorInteract(this);
        TryOpen();
    }

    protected override void ResetObject()
    {
        open = _initialOpen;
        locked = _initialLocked;
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

    public void TriggerLockAndClose()
    {
        _triggerLockAndClose = true;
    }
}