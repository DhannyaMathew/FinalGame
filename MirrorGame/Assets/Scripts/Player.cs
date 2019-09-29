using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class Player : MonoBehaviour
{
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
            Movement.Move(_mainCamera.Theta);

        var ray = _mainCamera.Camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        ray.origin = _mainCamera.Target - Vector3.up * 0.9f;
        if (Input.GetButtonDown("Interact"))
        {
            Interactable.Interact(GameManager.CurrentLevel.Interactables, transform);
        }
    }

    public void Teleport(Transform teleporter, Transform target, bool flipForward = false)
    {
        var portalToPlayer = transform.position - teleporter.position;

        var dp = Vector3.Dot((flipForward ? -1 : 1) * teleporter.forward, portalToPlayer);
        if (dp < 0)
        {
            var newRot = target.TransformDirection(
                Quaternion.AngleAxis(180f, teleporter.up) *
                teleporter.InverseTransformDirection(transform.forward));

            var cameraRot = target.TransformDirection(
                Quaternion.AngleAxis(180f, teleporter.up) *
                teleporter.InverseTransformDirection(_mainCamera.transform.forward));


            transform.forward = newRot;
            _mainCamera.transform.forward = cameraRot;
            transform.position = target.position + (flipForward ? 1 : -1) * target.forward;
        }
    }
}