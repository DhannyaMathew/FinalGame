using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 4.5f;
    [SerializeField, Range(0.01f, 1f)] private float rotateLerpSpeed = 0.085f;
    [SerializeField, Range(0.01f, 1f)] private float accelerationFactor = 0.1f;
    [SerializeField] private bool debug;
    [SerializeField] private float maxGroundAngle;
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask ladder;

    private float _speed;
    private float _actualSpeed;
    private bool _isRunning;
    private Vector3 _moveDirection;
    private Animator _animator;
    private bool _grounded;
    private RaycastHit _hitInfo;
    private float _angle;
    private Rigidbody _rigidbody;


    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int YDir = Animator.StringToHash("yDir");
    private static readonly int XDir = Animator.StringToHash("xDir");

    private Quaternion _moveDirectionRotation;

    public Vector3 Forward { get; private set; }
    public float GroundAngle { get; private set; }

    private void Awake()
    {
        _speed = walkSpeed;
        _animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed = runSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = walkSpeed;
        }

        _moveDirection =
            Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal") / 2f, 0, Input.GetAxis("Vertical")), 1f);
        _moveDirectionRotation = _moveDirection.sqrMagnitude > Mathf.Epsilon
            ? Quaternion.LookRotation(_moveDirection)
            : Quaternion.identity;
    }

    public void Move(float cameraFlatAngle)
    {
        GetInput();
        //CalculateGroundAngle();
        DrawDebugLines();
        CalculateSpeed();
        UpdateAnimator();
        Rotate(cameraFlatAngle);
        //  CalculateForward();
        Move();
    }

    private void UpdateAnimator()
    {
        _animator.SetFloat(Speed, _actualSpeed * _moveDirection.magnitude / runSpeed);
        _animator.SetFloat(XDir, _moveDirection.x);
        _animator.SetFloat(YDir, _moveDirection.y);
    }

    private void Move()
    {
        if (GroundAngle > maxGroundAngle || !_grounded) return;
        _rigidbody.velocity = _moveDirection.magnitude * _actualSpeed * Forward;
    }

    private void Rotate(float cameraFlatAngle)
    {
        var direction = Quaternion.Euler(0, cameraFlatAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotateLerpSpeed);
    }

    private void CalculateSpeed()
    {
        _actualSpeed = Mathf.Lerp(_actualSpeed, _speed, accelerationFactor);
    }

    private void DrawDebugLines()
    {
        if (debug)
        {
            Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + Forward * 2f, Color.blue);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (CheckLayer(other.gameObject.layer, ground))
        {
            _grounded = false;
            Forward = transform.forward;
            GroundAngle = 0;
            foreach (var otherContact in other.contacts)
            {
                if (Vector3.Angle(otherContact.normal, _moveDirectionRotation * transform.forward) -
                    90 < maxGroundAngle)
                {
                    _grounded = true;
                    Forward = Vector3.Cross(otherContact.normal,
                        _moveDirectionRotation * -transform.right);
                    GroundAngle = Vector3.Angle(otherContact.normal,
                                      _moveDirectionRotation * transform.forward) -
                                  90;
                    Debug.DrawLine(otherContact.point, otherContact.point + otherContact.normal * 0.3f, Color.yellow);
                    return;
                }
            }
        }

    
    }

    private void OnCollisionExit(Collision other)
    {
        if (CheckLayer(other.gameObject.layer, ground))
        {
            _grounded = false;
            Forward = transform.forward;
            GroundAngle = 0;
        }
    }

    private bool CheckLayer(LayerMask input, LayerMask compare)
    {
        return (1 << input.value & compare.value) != 0;
    }
}