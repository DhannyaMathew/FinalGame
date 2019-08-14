using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject mirror;
    [SerializeField] private float hitOffset = 0.1f;

    private GameObject level;
    // Start is called before the first frame update
    void Start()
    {
        level = GameObject.FindGameObjectWithTag("Level");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit,
                Mathf.Infinity))
            {
                var m = Instantiate(mirror, level.transform);
                m.transform.position = hit.point + hit.normal * hitOffset;
                m.transform.rotation = Quaternion.LookRotation(hit.normal);
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance,
                    Color.yellow);
            }
        }
    }
}