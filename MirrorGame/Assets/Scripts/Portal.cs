using System;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private Camera mainCamera;
    public Camera portalCamera;
    public Transform pairPortal;

    private void Start()
    {
        mainCamera = FindObjectOfType<MainCamera>().GetComponent<Camera>();
    }

    void Update()
    {
        portalCamera.projectionMatrix = mainCamera.projectionMatrix; // Match matrices
        var relativePosition = transform.InverseTransformPoint(mainCamera.transform.position);
        relativePosition = Vector3.Scale(relativePosition, new Vector3(-1, 1, -1));
        portalCamera.transform.position = pairPortal.TransformPoint(relativePosition);

        var relativeRotation = transform.InverseTransformDirection(mainCamera.transform.forward);
        relativeRotation = Vector3.Scale(relativeRotation, new Vector3(-1, 1, -1));
        portalCamera.transform.forward = pairPortal.TransformDirection(relativeRotation);
    }
}