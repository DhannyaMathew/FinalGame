using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class Player : MonoBehaviour
{
    private GameObject _tempKey;
    private MainCamera _mainCamera;
    private Rigidbody _rigidbody;
    public PlayerMove Movement { get; private set; }
    public Shoot Shoot { get; private set; }
    public bool HasKey => _tempKey.activeSelf;

    private Animator _animator;
    private static readonly int Interact = Animator.StringToHash("Interact");
    private Timer _interactAnimationTimer;


    private void Awake()
    {
        Shoot = GetComponent<Shoot>();
        Movement = GetComponent<PlayerMove>();
        _animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _tempKey = transform.GetChild(1).gameObject;
    }

    public void Setup(MainCamera mainCamera)
    {
        _mainCamera = mainCamera;
        EventHandler.OnKeyPickUp += OnKeyPickUp;
        EventHandler.OnDoorInteract += OnDoorInteract;
        EventHandler.OnInteract += OnInteract;
        EventHandler.OnDoorWalkThrough += OnDoorWalkThrough;
        _interactAnimationTimer = new Timer(19f / 24f, false, () => { _animator.SetBool(Interact, false); });
    }

    private void OnDoorWalkThrough(Door door, Level.Transition transition)
    {
        if (transition == Level.Transition.NEXT)
        {
            door.TriggerLockAndClose();
        }
    }

    private void OnInteract()
    {
        //_animator.SetBool(Interact, true);
        //_interactAnimationTimer.StartTimer();
    }

    private void OnKeyPickUp(Key key)
    {
        key.gameObject.SetActive(false);
        _tempKey.SetActive(true);
    }

    private void OnDoorInteract(Door door)
    {
        if (HasKey && door.IsLocked)
        {
            door.Unlock();
            _tempKey.SetActive(false);
        }
    }

    private void Update()
    {
        if (!GameManager.Paused)
            Movement.Move(_mainCamera.Theta);

        var ray = _mainCamera.Camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        ray.origin = _mainCamera.Target - Vector3.up * 0.9f;
        Interactable.Interact(GameManager.CurrentLevel.Interactables, transform);
        _interactAnimationTimer.Tick(Time.deltaTime);

        if (transform.position.y < -50f)
        {
            if (EventHandler.OnFallOutOfMap != null)
                EventHandler.OnFallOutOfMap();
        }
    }

    public void Teleport(Transform teleporter, Transform target)
    {
        var angleDiff = target.eulerAngles - teleporter.eulerAngles;
        angleDiff.y += 180;
        _mainCamera.SetRotation(_mainCamera.Theta + angleDiff.y, _mainCamera.Phi + angleDiff.x);
        transform.Rotate(angleDiff);
        transform.position = target.position;
    }

    public void FallOffMap(Level level)
    {
        level.SetDefaultState();
        transform.position = level.StartPoint.transform.position + Vector3.up * 50f;
        _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
    }
}