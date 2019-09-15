using UnityEngine;


public class MainCamera : MonoBehaviour
{
    [SerializeField] private float maxDist = 5;
    [SerializeField] private float xSensitivity = 5;
    [SerializeField] private float ySensitivity = 5;
    [SerializeField] private float minAngle = -35f;
    [SerializeField] private float maxAngle = 45f;
    [SerializeField, Range(0.01f, 1f)] private float rotateLerpSpeed = 0.085f;
    [SerializeField, Range(0.01f, 1f)] private float cameraDistLerpSpeed = 0.3f;
    [SerializeField] private bool alwaysShowPlayer = false;
    public float FlatDirection { get; private set; }
    public Camera Camera { get; private set; }

    private Transform _target;
    private float _dist;
    private float _actualDist;

    private float _acutalTheta;
    private float _phi = -35;
    private float _acutalPhi = -35;

    public Vector3 Target => _target.position + Vector3.up * 1.8f;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _target = GameManager.PlayerTransform;
        FlatDirection = 90;
        _acutalTheta = FlatDirection;
        Camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Paused)
        {
            var diff = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            FlatDirection += diff.x * xSensitivity;
            _phi += diff.y * ySensitivity;
            _phi = Mathf.Clamp(_phi, minAngle, maxAngle);
            _acutalPhi = Mathf.Lerp(_acutalPhi, _phi, rotateLerpSpeed);
            _acutalTheta = Mathf.Lerp(_acutalTheta, FlatDirection, rotateLerpSpeed);
            var offset = Quaternion.AngleAxis(_acutalTheta, Vector3.up) *
                         Quaternion.AngleAxis(-_acutalPhi, Vector3.right) *
                         Vector3.forward;

            var targetPos = Target;
            var ray = new Ray(targetPos, -offset);
            _dist = maxDist;
            if (alwaysShowPlayer)
            {
                if (Physics.Raycast(ray, out var intersectCheck, maxDist))
                    _dist = intersectCheck.distance - 0.01f;
            }

            _actualDist = Mathf.Lerp(_actualDist, _dist, cameraDistLerpSpeed);
            transform.position = targetPos - _actualDist * offset;
            transform.LookAt(targetPos);
        }
    }

    public void Mirror()
    {
        FlatDirection *= -1;
        _acutalTheta *= -1;
    }
}