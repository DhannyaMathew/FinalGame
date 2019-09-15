using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    public Transform playerCamera;
    public Transform portalA;
    public Transform portalB;
    public Material material;
    

    // Update is called once per frame
    void Update()
    {
        var playerCamOffsetFromPortal = playerCamera.position - portalA.position;
        transform.position = portalB.position + playerCamOffsetFromPortal;
                                                                            
        var angularDifference = portalA.eulerAngles - portalB.eulerAngles;
        var portalRotationalDiff = Quaternion.Euler(angularDifference.x, 0, angularDifference.z);
        var forward = playerCamera.forward;
        var newCamDir = portalRotationalDiff * forward;
        transform.rotation = Quaternion.LookRotation(newCamDir);
    }
}
