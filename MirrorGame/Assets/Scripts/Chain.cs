using System;
using UnityEngine;

public class Chain : MonoBehaviour
{
    [SerializeField] private float maxVelocity = 4;
    [SerializeField] private Vector2 minMaxDrag;
    [SerializeField] private AnimationCurve veloctiiyDrag;
    [SerializeField] private float maxAngularVelocity = 4;
    [SerializeField] private Vector2 minMaxAngularDrag;
    [SerializeField] private AnimationCurve angularVeloctiiyAngularDrag;

    private AudioSource _soundFalling;
    [SerializeField] public float soundThresh = 1f;

    private Link[] _links;

    private void Start()
    {
        _links = GetComponentsInChildren<Link>();
        _soundFalling = GetComponent<AudioSource>();
        var dragLm = new LinearMapping {minMaxIn = Vector2.up, minMaxOut = minMaxDrag};
        var angularDragLm = new LinearMapping {minMaxIn = Vector2.up, minMaxOut = minMaxAngularDrag};
        foreach (var link in GetComponentsInChildren<Link>())
        {
            link.SetPhysics(veloctiiyDrag, angularVeloctiiyAngularDrag, dragLm, angularDragLm, maxVelocity,
                maxAngularVelocity);
        }
    }

    private void Update()
    {
        bool play =false;
        foreach (var link in _links)
        {
            play = play || link.AboveSoundThresh;
        }

        if (play)
        {
            PlaySound();
        }
        else
            _soundFalling.Stop();
    }

    public void PlaySound()
    {
        if (!_soundFalling.isPlaying)
        {
            _soundFalling.Play();
        }
    }
}