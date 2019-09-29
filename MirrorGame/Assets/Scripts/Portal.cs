using System;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private Portal _otherPortal;
    private Material _portalMaterial;
    private Camera _exitPortalCamera;
    private static readonly int PortalTexture = Shader.PropertyToID("_PortalTexture");

    private bool _justTeleported;
    private Transform RootTransform => transform;
    private Door _door;
    private Timer _portalResetTimer;

    private void Start()
    {
        _door = GetComponentInParent<Door>();
        _portalResetTimer = new Timer(1f, false, ResetTeleporter);
    }

    private void ResetTeleporter()
    {
        _justTeleported = true;
    }

    private void TeleportPlayer()
    {
        GameManager.Player.Teleport(RootTransform, _otherPortal.RootTransform, !_door.IsEntrance);
        _justTeleported = true;
        _portalResetTimer.StartTimer();
        EventHandler.OnDoorWalkThrough(_door.IsEntrance ? Level.Transition.PREV : Level.Transition.NEXT);
    }

    public void UpdatePortalCamera(Camera camera)
    {
        _exitPortalCamera.projectionMatrix = camera.projectionMatrix; // Match matrices
        var pairPortal = _otherPortal.RootTransform;
        var flipOtherCamera = Mathf.Sign(pairPortal.lossyScale.x);
        var relativePosition = RootTransform.InverseTransformPoint(camera.transform.position);
        var relativeForward = RootTransform.InverseTransformDirection(camera.transform.forward);
        var relativeUp = RootTransform.InverseTransformDirection(camera.transform.up);
        relativePosition = Vector3.Scale(relativePosition, new Vector3(-1, 1, -1));
        relativeForward = Vector3.Scale(relativeForward, new Vector3(flipOtherCamera, 1, flipOtherCamera));
        relativeUp = Vector3.Scale(relativeUp, new Vector3(flipOtherCamera, 1, flipOtherCamera));

        var relativeRotation = Quaternion.LookRotation(relativeForward, relativeUp);
        relativeForward = pairPortal.InverseTransformDirection(relativeRotation * Vector3.forward);
        relativeUp = pairPortal.InverseTransformDirection(relativeRotation * Vector3.up);
        _exitPortalCamera.transform.position = pairPortal.TransformPoint(relativePosition);
        _exitPortalCamera.transform.rotation = Quaternion.LookRotation(relativeForward, relativeUp);

        _portalMaterial.SetInt("_FlipX", RootTransform.lossyScale.x * pairPortal.lossyScale.x > 0 ? 0 : 1);
    }

    public void SetOtherPortal(Portal other)
    {
        _otherPortal = other;
        _exitPortalCamera = transform.GetChild(0).GetComponent<Camera>();
        _portalMaterial = transform.GetChild(1).GetComponent<Renderer>().material;
        if (_exitPortalCamera.targetTexture != null)
        {
            _exitPortalCamera.targetTexture.Release();
        }

        _exitPortalCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        _portalMaterial.SetTexture(PortalTexture, _exitPortalCamera.targetTexture);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_otherPortal._justTeleported)
            {
                TeleportPlayer();
            }
        }
    }

    private void OnDestroy()
    {
        if (_exitPortalCamera.targetTexture != null)
        {
            _exitPortalCamera.targetTexture.Release();
            _exitPortalCamera.targetTexture = null;
        }
    }
}