using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float maxDist = 5;
    [SerializeField] private float xSensitivity = 5;
    [SerializeField] private float ySensitivity = 5;
    [SerializeField] private float minAngle = -35f;
    [SerializeField] private float maxAngle = 45f;
    [SerializeField, Range(0.01f, 1f)] private float rotateLerpSpeed = 0.085f;
    [SerializeField, Range(0.01f, 1f)] private float cameraDistLerpSpeed = 0.3f;

    public Quaternion FlatDirection => Quaternion.AngleAxis(_theta, Vector3.up);
    
    private float _dist;
    private float _actualDist;
    
    private float _theta;
    private float _acutalTheta;
    
    private float _phi;
    private float _acutalPhi;
    
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Paused)
        {
            var diff = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            _theta += diff.x * xSensitivity;
            _phi += diff.y * ySensitivity;
            _phi = Mathf.Clamp(_phi, minAngle, maxAngle);
            _acutalPhi = Mathf.Lerp(_acutalPhi, _phi, rotateLerpSpeed);
            _acutalTheta = Mathf.Lerp(_acutalTheta, _theta, rotateLerpSpeed);
        }

       
    }

    private void FixedUpdate()
    {
        if (!GameManager.Paused)
        {
            var offset = Quaternion.AngleAxis(_acutalTheta, Vector3.up) *
                         Quaternion.AngleAxis(-_acutalPhi, Vector3.right) *
                         Vector3.forward;

            var targetPos = target.position + Vector3.up;
            var ray = new Ray(targetPos, -offset);
            
            if (Physics.Raycast(ray, out var intersectCheck, maxDist))
                _dist = intersectCheck.distance - 0.01f;
            else
                _dist = maxDist;

            _actualDist = Mathf.Lerp(_actualDist, _dist, cameraDistLerpSpeed);
            transform.position = targetPos - _actualDist * offset;
            transform.LookAt(targetPos);
        }
    }
}