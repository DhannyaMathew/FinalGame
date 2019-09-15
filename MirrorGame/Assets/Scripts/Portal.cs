using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using RenderPipeline = UnityEngine.Experimental.Rendering.RenderPipeline;


public class Portal : MonoBehaviour
{
    private Portal _exitPortal;
    private Material _portalMaterial;
    private Transform _mainCameraTransform;
    private Camera _exitPortalCamera;
    private static readonly int PortalTexture = Shader.PropertyToID("_PortalTexture");

    public Vector3 PlayerToPortalOffset => _mainCameraTransform.position - transform.position;
    private bool _teleportPlayer, _justTeleported;

    private void Update()
    {
        UpdatePortalCamera();
        if (_teleportPlayer)
        {
            var portalToPlayer = GameManager.Player.transform.position - transform.position;
            var dp = Vector3.Dot(transform.up, portalToPlayer);
            if (dp < 0f)
            {
                var rd = Quaternion.Angle(transform.rotation, _exitPortal.transform.rotation);
                rd += 180;
                GameManager.Player.transform.Rotate(Vector3.up, rd);
                var po = Quaternion.Euler(0, rd, 0) * portalToPlayer;
                GameManager.Player.transform.position = _exitPortal.transform.position + po;
                _teleportPlayer = false;
            }
        }
    }

    private void UpdatePortalCamera()
    {
        var angularDifference = transform.eulerAngles - _exitPortal.transform.eulerAngles;
        var portalRotationalDiff = Quaternion.Euler(angularDifference.x, -angularDifference.y, angularDifference.z);
        _exitPortalCamera.transform.position =
            _exitPortal.transform.position + portalRotationalDiff * PlayerToPortalOffset;
        var forward = _mainCameraTransform.forward;
        var newCamDir = portalRotationalDiff * forward;
        _exitPortalCamera.transform.rotation = Quaternion.LookRotation(newCamDir);
    }

    public void SetExitPortal(Portal exit)
    {
        _exitPortal = exit;
        _portalMaterial = transform.GetChild(transform.childCount - 1).GetComponent<Renderer>().material;
        _mainCameraTransform = GameManager.MainCamera.transform;
        _exitPortalCamera = transform.GetChild(transform.childCount - 2).GetComponent<Camera>();
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
                _exitPortal._justTeleported = true;
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