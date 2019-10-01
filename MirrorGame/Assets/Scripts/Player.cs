﻿using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class Player : MonoBehaviour
{
    [CanBeNull] private Key _key;
    private Transform _keyHold;
    private MainCamera _mainCamera;
    public PlayerMove Movement { get; private set; }
    public bool HasKey => _key != null;

    private Animator _animator;
    private static readonly int Interact = Animator.StringToHash("Interact");
    private Timer _interactAnimationTimer;

    public void Setup(MainCamera mainCamera)
    {
        _mainCamera = mainCamera;
        EventHandler.OnKeyPickUp += OnKeyPickUp;
        EventHandler.OnDoorInteract += OnDoorInteract;
        EventHandler.OnInteract += OnInteract;
        _interactAnimationTimer = new Timer(19f / 24f, false, () => { _animator.SetBool(Interact, false); });
    }

    private void OnInteract()
    {
        _animator.SetBool(Interact, true);
        _interactAnimationTimer.StartTimer();
    }

    private void OnKeyPickUp(Key key)
    {
        _key = key;
        key.ChildTo(_keyHold);
    }

    private void OnDoorInteract(Door door)
    {
        if (HasKey && door.IsLocked)
        {
            door.Unlock();
            _key.ChildTo(door.KeyHole);
            _key = null;
        }
    }

    private void Awake()
    {
        Movement = GetComponent<PlayerMove>();
        _keyHold = GameObject.FindWithTag("KeyHold").transform;
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!GameManager.Paused)
            Movement.Move(_mainCamera.Theta);

        var ray = _mainCamera.Camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        ray.origin = _mainCamera.Target - Vector3.up * 0.9f;
        if (Input.GetButtonDown("Interact"))
        {
            Interactable.Interact(GameManager.CurrentLevel.Interactables, transform);
        }

        _interactAnimationTimer.Tick(Time.deltaTime);
    }

    public void Teleport(Transform teleporter, Transform target)
    {
        var angleDiff = target.eulerAngles - teleporter.eulerAngles;
        angleDiff.y += 180;
        _mainCamera.SetRotation(_mainCamera.Theta + angleDiff.y, _mainCamera.Phi + angleDiff.x);
        transform.Rotate(angleDiff);
        transform.position = target.position;
    }
}