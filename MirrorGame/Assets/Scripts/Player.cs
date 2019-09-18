using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask interactables;
    [SerializeField] private float interactDistance = 1.5f;

    private MainCamera _mainCamera;
    private PlayerMove Movement { get; set; }

    public void Setup(MainCamera mainCamera)
    {
        _mainCamera = mainCamera;
    }

    private void Awake()
    {
        Movement = GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Paused)
            Movement.Move(_mainCamera.FlatDirection);

        var ray = _mainCamera.Camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        ray.origin = _mainCamera.Target - Vector3.up * 0.9f;
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * interactDistance, Color.yellow);
        if (Input.GetButtonDown("Interact"))
        {
            if (Physics.Raycast(ray, out var hit, interactDistance, interactables))
            {
                var interactable = hit.transform.gameObject.GetComponent<Interactable>();
                if (interactable == null)
                {
                    interactable = hit.transform.gameObject.GetComponentInChildren<Interactable>();
                }

                if (interactable == null)
                {
                    interactable = hit.transform.gameObject.GetComponentInParent<Interactable>();
                }

                if (interactable != null)
                {
                    interactable.OnInteract();
                }
                else
                {
                    Debug.LogWarning("Object on interactable layer is not interactable");
                }
            }
        }
    }

    public bool Teleport(Transform teleporter, Transform target)
    {
        var portalToPlayer = transform.position - teleporter.position;
        var dp = Vector3.Dot(teleporter.up, portalToPlayer);
        if (dp < 0)
        {
            var rd = Quaternion.Angle(teleporter.rotation, target.rotation)+180;
            transform.Rotate(Vector3.up, rd);
            var po = Quaternion.Euler(0, rd, 0) * portalToPlayer;
            transform.position = target.position + po;
            return true;
        }

        return false;
    }
}