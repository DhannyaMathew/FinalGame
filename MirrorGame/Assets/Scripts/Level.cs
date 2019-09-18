﻿using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Level : MonoBehaviour
{
    [SerializeField] private Door entrance;
    [SerializeField] private Door exit;
    [SerializeField] private VolumeProfile sceneSettings;
    [SerializeField] private VolumeProfile postFX;
    [SerializeField] private ReflectionAttribute reflectionAttribute;
    private bool _hasOrb;
    private Mirror[] _mirrors;
    private Chain[] _chains;
    private Light[] _lights;
    private StartPoint _levelStart;
    private OrbPath _orbPath;
    private bool _resetMirrors;

    public VolumeProfile PostFx => postFX;
    public VolumeProfile SceneSettings => sceneSettings;

    private void Awake()
    {
        _levelStart = GetComponentInChildren<StartPoint>();
        _orbPath = GetComponentInChildren<OrbPath>();
        if (_orbPath == null) _hasOrb = false;
        _mirrors = GetComponentsInChildren<Mirror>();
        _chains = GetComponentsInChildren<Chain>();
        _lights = GetComponentsInChildren<Light>();
        reflectionAttribute.Set();
    }

    private void Update()
    {
        MirrorResetCheck();
    }

    public void Reflect()
    {
        reflectionAttribute.Reflect();
    }


    private void MirrorResetCheck()
    {
        if (_resetMirrors)
        {
            TurnOnMirrors();
            _resetMirrors = false;
        }
    }
    
    public void TurnOffMirrors()
    {
        foreach (var mirror in _mirrors)
        {
            mirror.gameObject.SetActive(false);
        }
    }

    public void TurnOnMirrors()
    {
        foreach (var mirror in _mirrors)
        {
            mirror.gameObject.SetActive(true);
        }
    }

    public void ResetMirrors()
    {
        TurnOffMirrors();
        _resetMirrors = true;
    }

    public void Setup(Player player, MainCamera mainCamera, Orb orb)
    {
        orb.transform.parent = transform;
        player.Setup(mainCamera);
        _levelStart.ResetPlayer(player);
        if (_hasOrb)
            orb.Set(_orbPath);
        else
            orb.gameObject.SetActive(false);
    }

    public void Link(Level level)
    {
        if (exit != null && level.entrance != null)
        {
            exit.Link(level.entrance, false);
            level.entrance.Link(exit, true);
        }
    }

    private void OnDisable()
    {
        reflectionAttribute.OnDisable();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void TurnOffDirectionalLights()
    {
        foreach (var light in _lights)
        {
            if (light.type == LightType.Directional)
            {
                light.gameObject.SetActive(false);
            }
        }
    }

    public void TurnOnDirectionalLights()
    {
        foreach (var light in _lights)
        {
            if (light.type == LightType.Directional)
            {
                Debug.Log(light.gameObject.name);
                light.gameObject.SetActive(true);
            }
        }
    }
}