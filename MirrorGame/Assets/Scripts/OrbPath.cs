using UnityEngine;

public class OrbPath : MonoBehaviour
{
    private int _currentWaypoint;
    public bool PathCompleted => _currentWaypoint == transform.childCount - 1;
    public Vector3 Waypoint => transform.GetChild(_currentWaypoint).position;
    public Vector3 StartPosition => transform.position;

    public void ToNextWaypoint()
    {
        _currentWaypoint++;
        _currentWaypoint = Mathf.Clamp(_currentWaypoint, 0, transform.childCount-1);
    }
}