using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MirrorProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject mirrorPrefab;
    private Vector3 _target = Vector3.zero;
    private Vector3 _normal = Vector3.forward;
    private Vector3 _forward;
    private bool _shot, _grow;
    private Vector3 _initialPosition;
    private Vector3 _initialScale;

    private void Start()
    {
        _initialPosition = transform.localPosition;
        _initialScale = transform.localScale;
    }


    void Update()
    {
        if (_shot)
        {
            transform.parent = GameManager.CurrentLevel.transform;
            transform.position = Vector3.Lerp(transform.position, _target, speed * Time.deltaTime);
            if ((transform.position - _target).sqrMagnitude < 0.2f * 0.2f)
            {
                var mirrorObj = Instantiate(mirrorPrefab, GameManager.CurrentLevel.transform);
                var mirror = mirrorObj.GetComponent<Mirror>();
                mirror.transform.position = _target + _normal * 0.001f;
                mirror.transform.rotation = Quaternion.LookRotation(_normal);
                mirror.CanBeInteractedWith = GameManager.CanPickupMirrors;
                mirror.SetLevel(GameManager.CurrentLevel);
                mirror.Level.ResetMirrors();
                mirror.DissolveAmount = 1f;
                mirror.FadeIn();
                gameObject.transform.parent = GameManager.Player.transform;
                gameObject.transform.localPosition = _initialPosition;
                _shot = false;
                gameObject.SetActive(false);
            }
        }

        if (_grow)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _initialScale, 5 * Time.deltaTime);
            if (Mathf.Abs(transform.localScale.x - _initialScale.x) < 0.05f)
            {
                transform.localScale = _initialScale;
                _grow = false;
            }
        }
    }

    public void Shoot(Vector3 target, Vector3 normal, Vector3 forward)
    {
        _target = target;
        _normal = normal;
        _forward = Vector3.Lerp(Vector3.ProjectOnPlane(forward, normal), Vector3.up,
            Mathf.Abs(Vector3.Dot(Vector3.forward, normal)));
        _shot = true;
    }

    public void Load()
    {
        gameObject.SetActive(true);
    }

    public void Grow()
    {
        transform.localScale = Vector3.zero;
        _grow = true;
    }
}