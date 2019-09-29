using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLight : MonoBehaviour
{
    private float _intialY;
    private float _intialXRot;

    private void Start()
    {
        _intialY = transform.position.y;
        _intialXRot = transform.localRotation.x*Mathf.Rad2Deg;
    }


    // Update is called once per frame
    void Update()
    {
        var player = GameManager.Player.transform.position;
        transform.position = new Vector3(player.x, _intialY, player.z);
        transform.LookAt(transform.parent.position);
        transform.eulerAngles = new Vector3(_intialXRot, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}