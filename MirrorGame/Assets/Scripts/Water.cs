using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : LevelObject
{
    private ParticleSystem _ps;
    [SerializeField] private ParticleSystem other;
    [SerializeField] private PressurePlate plate;
    [SerializeField] private GameObject iceBlock;
    private bool _isWater, _animateWater;
    private AudioSource _source;
    private GameObject ice, water;
    private int count;
    private bool playParticleAndSound;
    public static bool canBePressed = true;
    private Vector3 initialWaterPos;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _source = GetComponent<AudioSource>();
        _ps = GetComponent<ParticleSystem>();
        ice = transform.GetChild(1).gameObject;
        water = transform.GetChild(2).gameObject;
        initialWaterPos = water.transform.localPosition;
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
                canBePressed = true;
            }
        };
    }

    private void Update()
    {
        if (_animateWater)
        {
            var p = _ps.time / _ps.main.duration;
            water.transform.localPosition = initialWaterPos + 0.1f*(1-p)*Vector3.forward;
            if (p>=0.99f)
            {
                water.transform.localPosition = initialWaterPos;
                _animateWater = false;
                plate.PressDown();
            }
        }

        
    }

    public void Reflect()
    {
     
        Debug.Log(count);
        if (count == 0)
        {
            _source.Play();
            _ps.Play();
            iceBlock.SetActive(false);
            _animateWater = true;
            canBePressed = false;
            ice.SetActive(false);
            water.SetActive(true);
            _isWater = !_isWater;
        }

        if (count > 0)
        {   _isWater = !_isWater;
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
        iceBlock.SetActive(true);
        _isWater = false;
        Debug.Log(count);
        canBePressed = true;
    }
}