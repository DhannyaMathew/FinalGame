﻿using System;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private Portal _otherPortal;
    private Material _portalMaterial;
    private Camera _exitPortalCamera;
    private static readonly int PortalTexture = Shader.PropertyToID("_PortalTexture");

    private bool _teleportPlayer, _justTeleported;
    private Transform RootTransform => transform;
    private Door _door;

    private void Start()
    {
        _door = GetComponentInParent<Door>();
    }

    private void Update()
    {
        if (_teleportPlayer)
        {
            TeleportPlayer();
        }
    }

    private void TeleportPlayer()
    {
        Level.Transition transition = Level.Transition.NONE;
        _teleportPlayer = !GameManager.Player.Teleport(transform, _otherPortal.transform);
        if (_door.IsEntrance)
        {
            transition = Level.Transition.PREV;
            _door.ForceOpen();
        }
        else
        {
            transition = Level.Transition.NEXT;
            _door.ForceOpen();
        }
        EventHandler.OnDoorWalkThrough(transition);
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
            if (!_justTeleported)
            {
                _teleportPlayer = true;
                _otherPortal._justTeleported = true;
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