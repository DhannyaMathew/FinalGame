using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    

    
    void Update()
    {
        var move = speed * Time.deltaTime *
                   transform.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0,
                       Input.GetAxisRaw("Vertical")));
        transform.position += move;
    }

    public void MirrorEnter(Vector3 normal)
    {
        var playerForward = transform.forward;
        transform.rotation = Quaternion.LookRotation(playerForward -
                                                     2 * Vector3.Dot(playerForward, normal) * normal);
    }
}