using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] private GameObject path;
    [SerializeField] private GameObject endEffect;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float startDist = 1f;
    private int _currentWaypoint;
    private bool _start;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if ((GameManager.PlayerTransform.position - transform.position).magnitude < startDist)
        {
            _start = true;
        }

        if (_start)
        {
            if (_currentWaypoint < path.transform.childCount)
            {
                var diff = path.transform.GetChild(_currentWaypoint).position - transform.position;
                transform.position += speed * Time.deltaTime * diff.normalized;
                if (diff.magnitude < 0.5f)
                {
                    _currentWaypoint++;
                    //Debug.Log(_currentWaypoint);
                }
            }
            else
            {
                Instantiate(endEffect, transform.position, Quaternion.Euler(-90, 0, 0));
                Destroy(gameObject);
            }
        }
    }
}