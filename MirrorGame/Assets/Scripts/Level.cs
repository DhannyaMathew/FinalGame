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
    private bool _hasOrb;
    private Mirror[] _mirrors;
    private Light[] _lights;
    private StartPoint _levelStart;
    private OrbPath _orbPath;
    private bool _resetMirrors;
    private Interactable[] _interactables;
    private ParticleSystem[] _particleSystems;

    public Interactable[] Interactables => _interactables;

    private void Awake()
    {
        _particleSystems = GetComponentsInChildren<ParticleSystem>();
        _levelStart = GetComponentInChildren<StartPoint>();
        _orbPath = GetComponentInChildren<OrbPath>();
        if (_orbPath == null) _hasOrb = false;
        _mirrors = GetComponentsInChildren<Mirror>();
        _lights = GetComponentsInChildren<Light>();
        _interactables = GetComponentsInChildren<Interactable>();
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
}