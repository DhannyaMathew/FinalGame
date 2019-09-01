using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] private GameObject startObject;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private GameObject endEffect;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float startDist = 1f;

    private bool _start;
    private AudioSource _audioSource;
    private Rigidbody _rb;

    // Start is called before the first frame update
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((GameManager.PlayerTransform.position - transform.position).magnitude < startDist)
        {
            _start = true;
            _audioSource.Play();
        }

        if (_start)
        {
            var seek = Seek();
            var avoid = Avoid();
            _rb.AddForce(2f*seek);
            _rb.AddForce( avoid);
        }
    }

    private Vector3 Seek()
    {
        var desired = targetObject.transform.position - transform.position;
        if (desired.magnitude < 0.5f)
        {
            Instantiate(endEffect, transform.position, Quaternion.Euler(-90, 0, 0));
            transform.Translate(0, 100, 0);
            _start = false;
            _audioSource.Stop();
        }
        desired.Normalize();
        desired *= speed;
        return Vector3.ClampMagnitude(desired - _rb.velocity, 1000f);
    }

    private Vector3 Avoid()
    {
        var diff = transform.position - (GameManager.PlayerTransform.position + Vector3.up * 0.9f);
        var d = diff.magnitude;
        if (d < 2f)
        {
            diff.Normalize();
            diff *= 2 * speed;
            diff -= _rb.velocity;
            if (diff.y < 0)
            {
                diff.y *= -1;
            }
            return Vector3.ClampMagnitude(diff, 1000f);
        }
        return Vector3.zero;
    }

    public void Respawn()
    {
        if (!_start)
        {
            transform.position = startObject.transform.position;
            _rb.velocity = Vector3.zero;
        }
    }
}