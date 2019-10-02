using System;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private bool _hasMirror;
    private MirrorProjectile _mirrorProjectile;

    private void Awake()
    {
        _mirrorProjectile = GetComponentInChildren<MirrorProjectile>();
        _mirrorProjectile.gameObject.SetActive(false);
    }

    private void Start()
    {
        EventHandler.OnMirrorPickup += () =>
        {
            _mirrorProjectile.Load();
            _hasMirror = true;
        };
        EventHandler.OnMirrorAbsorb += () =>
        {
            _mirrorProjectile.gameObject.SetActive(true);
            _mirrorProjectile.Grow();
            _hasMirror = true;
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && _hasMirror)
        {
            RaycastHit hit;
            var ray = GameManager.MainCamera.Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position + Vector3.up,
                ray.direction, out hit,
                Mathf.Infinity))
            {
                _hasMirror = false;
                _mirrorProjectile.Shoot(hit.point, hit.normal);
                GameManager.CurrentLevel.ResetMirrors();
            }
        }
    }
}