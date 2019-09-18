using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool debug;
    private Portal _otherPortal;
    private Material _portalMaterial;
    private Camera _exitPortalCamera;
    private static readonly int PortalTexture = Shader.PropertyToID("_PortalTexture");

    private bool _teleportPlayer, _justTeleported;

    private Transform RootTransform => transform;

    private void Update()
    {
        if (_teleportPlayer)
        {
            TeleportPlayer();
        }
    }

    private void TeleportPlayer()
    {
        _teleportPlayer = !GameManager.Player.Teleport(transform, _otherPortal.transform);
    }

    public void UpdatePortalCamera(Camera camera)
    {
        _exitPortalCamera.projectionMatrix = camera.projectionMatrix; // Match matrices
        var flip = new Vector3(-1, 1, -1);
        var pairPortal = _otherPortal.RootTransform;
        var relativePosition = RootTransform.InverseTransformPoint(camera.transform.position);
        var relativeForward = RootTransform.InverseTransformDirection(camera.transform.forward);
        var relativeUp = RootTransform.InverseTransformDirection(camera.transform.up);
      
        relativePosition = Vector3.Scale(relativePosition, flip);
        /*relativeForward = Vector3.Scale(relativeForward, flip);
        relativeUp = Vector3.Scale(relativeUp, flip);*/
        
        var relativeRotation = Quaternion.LookRotation(relativeForward, relativeUp);
        relativeForward = pairPortal.InverseTransformDirection(relativeRotation * Vector3.forward);
        relativeUp = pairPortal.InverseTransformDirection(relativeRotation * Vector3.up);
        _exitPortalCamera.transform.position = pairPortal.TransformPoint(relativePosition);
        _exitPortalCamera.transform.rotation = Quaternion.LookRotation(relativeForward, relativeUp);
        var FlipX = RootTransform.lossyScale.x * pairPortal.lossyScale.x < 0 ? 1 : 0;
        _portalMaterial.SetInt("_FlipX", FlipX);
        Debug.Log(FlipX);
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
            if (!_justTeleported)
            {
                _teleportPlayer = true;
                _otherPortal._justTeleported = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_justTeleported)
            {
                _justTeleported = false;
            }
        }
    }
}