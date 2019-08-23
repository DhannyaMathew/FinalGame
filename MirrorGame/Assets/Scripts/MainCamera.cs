using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float maxDist = 5;
    [SerializeField] private float lerpSpeed = 0.3f;
    [SerializeField] private float xSensitivity = 5;
    [SerializeField] private float ySensitivity = 5;
    [SerializeField] private float minAngle = -35f;
    [SerializeField] private float maxAngle = 45f;
    
    public Quaternion FlatDirection => Quaternion.AngleAxis(_theta, Vector3.up);
    
    private float _dist;
    private float _theta = 0;
    private float _phi = 0;
    private float _actualDist = 0;
    private float _acutalTheta = 0;
    private float _acutalPhi = 0;
    private Vector3 _previousMouse;
    private bool _move = true;


    public bool Move => _move;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (_move)
        {
            var diff = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            _theta += diff.x * xSensitivity;
            _phi += diff.y * ySensitivity;
            _phi = Mathf.Clamp(_phi, minAngle, maxAngle);
            _acutalPhi = Mathf.LerpAngle(_acutalPhi, _phi, lerpSpeed);
            _acutalTheta = Mathf.LerpAngle(_acutalTheta, _theta, lerpSpeed);
            
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _move = !_move;
            Cursor.visible = !_move;
            Cursor.lockState = _move ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

    private void FixedUpdate()
    {
        if (_move)
        {
            RaycastHit intersectCheck;


            var offset = Quaternion.AngleAxis(_acutalTheta, Vector3.up) *
                         Quaternion.AngleAxis(_acutalPhi, Vector3.right) *
                         Vector3.forward;

            var targetPos = target.position + Vector3.up;
            var ray = new Ray(targetPos, -offset);

            if (Physics.Raycast(ray, out intersectCheck, maxDist))
            {
                _dist = intersectCheck.distance - 0.01f;
            }
            else
            {
                _dist = maxDist;
            }

            _actualDist = Mathf.Lerp(_actualDist, _dist, lerpSpeed);
            transform.position = targetPos - _actualDist * offset;
            transform.LookAt(targetPos);
        }
    }
}