using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask interactables;
    [SerializeField] private float interactDistance = 1.5f;

    [CanBeNull] private Key _key;

    private Transform _keyHold;

    private MainCamera _mainCamera;
    public PlayerMove Movement { get; private set; }
    public bool HasKey => _key != null;

    public void Setup(MainCamera mainCamera)
    {
        _mainCamera = mainCamera;
        EventHandler.OnKeyPickUp += OnKeyPickUp;
        EventHandler.OnDoorInteract += OnDoorInteract;
    }

    private void OnKeyPickUp(Key key)
    {
        _key = key;
        key.ChildTo(_keyHold);
    }

    private void OnDoorInteract(Door door)
    {
        if (HasKey)
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
    }

    private void Update()
    {
        if (!GameManager.Paused)
            Movement.Move(_mainCamera.FlatDirection);

        var ray = _mainCamera.Camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        ray.origin = _mainCamera.Target - Vector3.up * 0.9f;
        if (Input.GetButtonDown("Interact"))
        {
            Interactable.Interact(GameManager.CurrentLevel.Interactables, transform);
        }
    }

    public bool Teleport(Transform teleporter, Transform target)
    {
        var portalToPlayer = transform.position - teleporter.position;
        var dp = Vector3.Dot(teleporter.up, portalToPlayer);
        if (dp < 0)
        {
            var rd = Quaternion.Angle(teleporter.rotation, target.rotation) + 180;
            transform.Rotate(Vector3.up, rd);
            var po = Quaternion.Euler(0, rd, 0) * portalToPlayer;
            transform.position = target.position + po;
            return true;
        }

        return false;
    }
}