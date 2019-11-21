using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using MainMenu;
using UnityEngine;

public class MirrorProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject mirrorPrefab;

    private RaycastHit _hit;
    private bool _shot, _grow, _shrink;
    private Vector3 _initialPosition;
    private Vector3 _initialScale;
    private GameObject _probe;
    public bool hasMirror => transform.localScale.magnitude > 0.1f && !_shot && !_grow;

    private void Awake()
    {
        _initialPosition = transform.localPosition;
        transform.localScale = Vector3.zero;
        _initialScale = new Vector3(0.4f,0.4f,0.4f);
        _probe = transform.GetChild(0).gameObject;
        Unload();
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
                transform.parent = GameManager.Player.transform;
                transform.localPosition = _initialPosition;
                transform.localScale = Vector3.zero;
                _shot = false;
            }
        }

        if (_grow)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _initialScale, 5 * Time.deltaTime);
            if (Mathf.Abs(transform.localScale.x - _initialScale.x) < 0.001f)
            {
                transform.localScale = _initialScale;
                _grow = false;

            }
        }

        if (_shrink)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 5 * Time.deltaTime);
            if (Mathf.Abs(transform.localScale.x) < 0.001f)
            {
                _probe.SetActive(false);
                transform.localScale = Vector3.zero;
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
        if (!hasMirror)
        {
            UiControl.ShowHintUI();
            Grow();
        }
    }
    
    private void Grow()
    {
        _probe.SetActive(true);
        transform.localScale = Vector3.zero;
        _grow = true;
        _shrink = false;
    }

    private void Shrink()
    {
        
        UiControl.HideHintUI();
        _shrink = true;
        _grow = false;
        transform.localScale = _initialScale;
    }

    public void Unload()
    {
        if (hasMirror)
        {
            Shrink();
        }
    }
}