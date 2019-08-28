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

    // Start is called before the first frame update
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
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
            var diff = targetObject.transform.position - transform.position;
            var oDiff = transform.position - (GameManager.PlayerTransform.position + Vector3.up * 0.5f);
            var dir = diff.normalized;
            if (oDiff.magnitude < 1.5f)
            {
                dir += oDiff.normalized;
            }

            transform.position += speed * Time.deltaTime * dir.normalized;

            if (diff.magnitude < 0.5f)
            {
                Instantiate(endEffect, transform.position, Quaternion.Euler(-90, 0, 0));
                transform.Translate(0, 100, 0);
                _start = false;
            }
        }
    }

    public void Respawn()
    {
        if (!_start)
        {
            transform.position = startObject.transform.position;
        }
    }
}