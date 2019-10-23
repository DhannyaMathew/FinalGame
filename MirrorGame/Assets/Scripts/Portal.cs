using System;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private bool _rotateBy180 = false;
    [SerializeField] private bool _rotateBy90 = false;
    private Portal _otherPortal;
    private Material _portalMaterial;
    private Camera _exitPortalCamera;
    private Door _door;
    private Exit _exit;
    private Timer _portalResetTimer;
    private bool _justTeleported;

    public Camera Camera { get; private set; }
    public GameObject PortalTarget { get; private set; }
    private Transform RootTransform => transform;
    private bool Exit => _exit != null;

    private static readonly int PortalTexture = Shader.PropertyToID("_PortalTexture");

    private void Start()
    {
        _exit = GetComponent<Exit>();
        _door = GetComponentInParent<Door>();
        Camera = GetComponentInChildren<Camera>();
        PortalTarget = transform.GetChild(1).gameObject;
        if (_portalResetTimer == null && _otherPortal != null)
        {
            _portalResetTimer = new Timer(0.1f, false, ResetTeleporter);
            _otherPortal._portalResetTimer = new Timer(0.1f, false, _otherPortal.ResetTeleporter);
        }
    }

    private void Update()
    {
        if (_otherPortal != null)
            _otherPortal._portalResetTimer.Tick(Time.deltaTime);
    }

    private void ResetTeleporter()
    {
        _otherPortal._justTeleported = false;
    }

    private void TeleportPlayer()
    {
        _justTeleported = true;
        GameManager.Player.Teleport(RootTransform, _otherPortal.RootTransform);
        _otherPortal._portalResetTimer.StartTimer();
        EventHandler.OnDoorWalkThrough(_otherPortal._door,
            _door.IsEntrance ? Level.Transition.PREV : Level.Transition.NEXT);
    }

    public void UpdatePortalCamera(Camera camera)
    {
        var flip = new Vector3(-1, 1, -1);
        _exitPortalCamera.projectionMatrix = camera.projectionMatrix;
        var pairPortal = _otherPortal.RootTransform;
        var flipOtherCamera = Mathf.Sign(pairPortal.lossyScale.x);
        var relativePosition = RootTransform.InverseTransformPoint(camera.transform.position);
        var relativeForward = RootTransform.InverseTransformDirection(camera.transform.forward);
        var relativeUp = RootTransform.InverseTransformDirection(camera.transform.up);
        relativePosition = Vector3.Scale(relativePosition, flip);
        relativeForward = Vector3.Scale(relativeForward, new Vector3(flipOtherCamera, 1, flipOtherCamera));
        relativeUp = Vector3.Scale(relativeUp, new Vector3(flipOtherCamera, 1, flipOtherCamera));
        var relativeRotation = Quaternion.LookRotation(relativeForward, relativeUp);
        relativeForward = pairPortal.InverseTransformDirection(relativeRotation * Vector3.forward);
        relativeUp = pairPortal.InverseTransformDirection(relativeRotation * Vector3.up);
        if (_rotateBy180)
        {
            relativeForward = Vector3.Scale(relativeForward, flip);
            relativeUp = Vector3.Scale(relativeUp, flip);
        }

        if (_rotateBy90)
        {
            relativeForward = Quaternion.AngleAxis(90, Vector3.up) * relativeForward;
            relativeUp = Quaternion.AngleAxis(90, Vector3.up) * relativeUp;
        }

        _portalMaterial.SetInt("_FlipX", RootTransform.lossyScale.x * pairPortal.lossyScale.x > 0 ? 0 : 1);
        _exitPortalCamera.transform.position = pairPortal.TransformPoint(relativePosition);
        _exitPortalCamera.transform.rotation = Quaternion.LookRotation(relativeForward, relativeUp);
    }

    public void SetOtherPortal(Portal other)
    {
        _otherPortal = other;
        _exitPortalCamera = transform.GetChild(0).GetComponent<Camera>();
        _portalMaterial = transform.GetChild(1).GetComponent<Renderer>().material;
        _portalMaterial.SetTexture(PortalTexture, _exitPortalCamera.targetTexture);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_otherPortal._justTeleported && _door.open && !Exit)
                TeleportPlayer();
        }
    }
}