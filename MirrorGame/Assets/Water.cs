using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : LevelObject
{
    private ParticleSystem _ps;
    [SerializeField] private ParticleSystem other;
    [SerializeField] private PressurePlate plate;
    private bool _isWater;
    private AudioSource _source;
    private GameObject ice, water;
    private int count;
    private bool playParticleAndSound;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _source = GetComponent<AudioSource>();
        _ps = GetComponent<ParticleSystem>();
        ice = transform.GetChild(1).gameObject;
        water = transform.GetChild(0).gameObject;
        ice.SetActive(false);
        water.SetActive(false);
        EventHandler.OnMirrorWalkThrough += mirror =>
        {
            if (Mathf.Abs(Vector3.Dot(mirror.transform.forward, Vector3.up)) > 0.25f && water.activeInHierarchy)
            {
                plate.Unpress();
                water.SetActive(false);
                other.Play();
                _source.Play();
            }
        };
    }

    public void Reflect()
    {
        if (count == 0)
        {
            _source.Play();
            _ps.Play();
        }

        if (count >= 0)
        {
            _isWater = !_isWater;
            ice.SetActive(!_isWater);
            water.SetActive(_isWater);
            plate.PressDown();
        }

        count++;
    }

    protected override void ResetObject()
    {
        count = -1;
        _ps.Stop();
        other.Stop();
        plate.Unpress();
        ice.SetActive(false);
        water.SetActive(false);
        _isWater = false;
    }
}