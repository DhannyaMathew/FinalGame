using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] private GameObject endEffect;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float startDist = 1f;

    private bool _start;
    private Rigidbody _rb;
    private OrbPath _path;

    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Set(OrbPath path)
    {
        _path = path;
        Respawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (_path != null)
        {
            if ((GameManager.Player.transform.position - transform.position).magnitude < startDist)
            {
                _start = true;
            }

            if (_start)
            {
                var seek = Seek();
                var avoid = Avoid();
                _rb.AddForce(2f * seek);
                _rb.AddForce(avoid);
            }
        }
    }

    private Vector3 Seek()
    {
        var desired = _path.Waypoint - transform.position;
        if (desired.magnitude < 0.5f)
        {
            _path.ToNextWaypoint();
            if (_path.PathCompleted)
            {
                Instantiate(endEffect, transform.position, Quaternion.Euler(-90, 0, 0));
                transform.Translate(0, 100, 0);
                _start = false;
            }
        }

        desired.Normalize();
        desired *= speed;
        return Vector3.ClampMagnitude(desired - _rb.velocity, 1000f);
    }

    private Vector3 Avoid()
    {
        var diff = transform.position - (GameManager.Player.transform.position + Vector3.up * 0.9f);
        var d = diff.magnitude;
        if (d < 2f)
        {
            diff.Normalize();
            diff *= 2 * speed;
            diff -= _rb.velocity;
            if (diff.y < 0)
            {
                diff.y *= -1;
            }

            return Vector3.ClampMagnitude(diff, 1000f);
        }

        return Vector3.zero;
    }

    public void Respawn()
    {
        _start = false;
        transform.position = _path.StartPosition;
        _rb.velocity = Vector3.zero;
    }
}