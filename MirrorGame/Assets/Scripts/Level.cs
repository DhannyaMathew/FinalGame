using System;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public enum Transition
    {
        NONE,
        PREV,
        NEXT,
        RANDOM
    }

    [SerializeField] private Door entrance;
    [SerializeField] private Door exit;
    [SerializeField] private Light mainDirectionalLight;
    [SerializeField] private AmbientLighting ambientLighting;
    [SerializeField] private bool ambientLightEnabled;
    [SerializeField] private Color ambientLight;
    [SerializeField] private float intensity = 1;
    [SerializeField] private ReflectionAttribute reflectionAttribute;
    [SerializeField] private bool hasOrb;
    public MainCameraSettings cameraSettings;
    private List<Mirror> _mirrors;
    private Light[] _lights;
    private StartPoint _levelStart;
    private OrbPath _orbPath;
    private bool _resetMirrors;
    private List<Interactable> _interactables;
    private ParticleSystem[] _particleSystems;
    private Quaternion _intialRotation;
    private Vector3 _initialPosition;

    public StartPoint StartPoint => _levelStart;
    public List<Interactable> Interactables => _interactables;

    private void Awake()
    {
        _particleSystems = GetComponentsInChildren<ParticleSystem>();
        _levelStart = GetComponentInChildren<StartPoint>();
        _orbPath = GetComponentInChildren<OrbPath>();
        if (_orbPath == null) hasOrb = false;
        _lights = GetComponentsInChildren<Light>();
        _initialPosition = transform.position;
        _intialRotation = transform.rotation;
        _mirrors = new List<Mirror>();
        _interactables = new List<Interactable>();
    }

    private void Start()
    {
        foreach (var mirror in GetComponentsInChildren<Mirror>())
        {
            _mirrors.Add(mirror);
        }

        foreach (var interactable in GetComponentsInChildren<Interactable>())
        {
            _interactables.Add(interactable);
        }
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

    private void TurnOffMirrors()
    {
        foreach (var mirror in _mirrors)
        {
            mirror.gameObject.SetActive(false);
        }
    }

    private void TurnOnMirrors()
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
        SetDefaultState();
        orb.transform.parent = transform;
        player.Setup(mainCamera);
        _levelStart.ResetPlayer(player);
        if (hasOrb)
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

    public void Activate()
    {
        gameObject.SetActive(true);
        TurnOnOtherLights();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void TurnOffDirectionalLight()
    {
        if (mainDirectionalLight != null)
            mainDirectionalLight.enabled = false;
    }

    public void TurnOnDirectionalLight()
    {
        if (ambientLighting != null)
            ambientLighting.SetValues(ambientLight, intensity, ambientLightEnabled);
        if (mainDirectionalLight != null)
            mainDirectionalLight.enabled = true;
    }

    public void TurnOnOtherLights()
    {
        foreach (var light in _lights)
        {
            if (light.type != LightType.Directional)
            {
                light.enabled = true;
            }
        }
    }

    public void TurnOffparticleSystems()
    {
        foreach (var system in _particleSystems)
        {
            system.gameObject.SetActive(false);
        }
    }

    public void TurnOnparticleSystems()
    {
        foreach (var system in _particleSystems)
        {
            system.gameObject.SetActive(true);
        }
    }

    public void TurnOffOtherLights()
    {
        foreach (var light in _lights)
        {
            if (light.type != LightType.Directional)
            {
                light.enabled = false;
            }
        }
    }

    public void UpdateAmbientLights()
    {
        if (ambientLighting != null)
            ambientLighting.SetValues(ambientLight, intensity, ambientLightEnabled);
    }

    public void SetDefaultState()
    {
        transform.position = _initialPosition;
        transform.rotation = _intialRotation;
        transform.localScale = new Vector3(1, 1, 1);
        reflectionAttribute.SetDefault();
    }

    private void OnDestroy()
    {
        SetDefaultState();
    }

    public void AddMirror(Mirror mirror)
    {
        _mirrors.Add(mirror);
        _interactables.Add(mirror);
    }

    public void RemoveMirror(Mirror mirror)
    {
        _mirrors.Remove(mirror);
        _interactables.Remove(mirror);
    }
}