using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MirrorProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject mirrorPrefab;
    private RaycastHit _hit;
    private bool _shot, _grow, _shrink, _isBig;
    private Vector3 _initialPosition;
    private Vector3 _initialScale;
    public bool isBig => _isBig;

    private void Awake()
    {
        _initialPosition = transform.localPosition;
        _initialScale = transform.localScale;
        Debug.Log(_initialScale);
    }


    void Update()
    {
        if (_shot)
        {
            transform.parent = GameManager.CurrentLevel.transform;
            transform.position = Vector3.Lerp(transform.position, _hit.point, speed * Time.deltaTime);
            if ((transform.position - _hit.point).sqrMagnitude < 0.1f * 0.1f)
            {
                var mirrorObj = Instantiate(mirrorPrefab, GameManager.CurrentLevel.transform);
                var mirror = mirrorObj.GetComponent<Mirror>();
                if (_hit.transform.gameObject.CompareTag("MirrorSpot"))
                {
                    mirror.transform.position = _hit.transform.parent.position;
                    mirror.transform.rotation = _hit.transform.parent.rotation;
                }
                else
                {
                    mirror.transform.position = _hit.point + _hit.normal * 0.001f;
                    mirror.transform.rotation = Quaternion.LookRotation(_hit.normal);
                }

                mirror.transform.parent = _hit.transform.parent;
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
            if (Mathf.Abs(transform.localScale.x - _initialScale.x) < 0.001f)
            {
                transform.localScale = _initialScale;
                _grow = false;
                _isBig = true;

            }
        }

        if (_shrink)
        {
            _isBig = false;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 5 * Time.deltaTime);
            if (Mathf.Abs(transform.localScale.x) < 0.001f)
            {
                transform.localScale = Vector3.zero;
                gameObject.SetActive(false);
                _shrink = false;
            }
        }
    }

    public void Shoot(RaycastHit hit)
    {
        _hit = hit;
        _shot = true;
    }

    public void Load()
    {
        Grow();
    }

    public void Grow()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        _grow = true;
        _shrink = false;
    }

    public void Shrink()
    {
        _shrink = true;
        _grow = false;
        transform.localScale = _initialScale;
    }
}