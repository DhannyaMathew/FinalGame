using UnityEngine;

public class OrbPath : MonoBehaviour
{
    private int _currentWaypoint;
    public bool PathCompleted { get; private set; }
    public Vector3 Waypoint => transform.GetChild(_currentWaypoint).position;
    public Vector3 StartPosition => transform.position;

    public void ToNextWaypoint()
    {
        _currentWaypoint++;
        if (_currentWaypoint == transform.childCount)
        {
            PathCompleted = true;
            _currentWaypoint--;
        }
    }
}