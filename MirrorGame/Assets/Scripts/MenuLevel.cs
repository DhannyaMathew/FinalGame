using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLevel : MonoBehaviour
{
    private float _intialY;
    private float _intialXRot;

    public int index = 0;
    private int _onDoorIndex;
    private float _winder = 360 * 500 + 88;
    private Vector2 _curr;
    private Vector2 _prev;


    public GameObject[] _allDoors;


    private void Start()
    {
        _intialY = transform.position.y;
        _intialXRot = transform.localRotation.x * Mathf.Rad2Deg;
        _curr = _prev = Vector2.zero;
        foreach (var door in _allDoors)
        {
            
            door.SetActive(false);
        }
        _allDoors[index].SetActive(true);
        _allDoors[(index+1)%_allDoors.Length].SetActive(true);
        _allDoors[(index+2)%_allDoors.Length].SetActive(true);
        _allDoors[(index+3)%_allDoors.Length].SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        var player = GameManager.Player.transform.position;
        transform.position = new Vector3(player.x, _intialY, player.z);
        transform.LookAt(transform.parent.position);
        transform.eulerAngles = new Vector3(_intialXRot, transform.eulerAngles.y, transform.eulerAngles.z);
        _curr = new Vector2(player.x, player.z) - new Vector2(-1000, 0);
        _winder -= Vector2.SignedAngle(_curr, _prev);

        _prev = _curr;
        var newIndex = ((int) _winder / 90 + 3) % (_allDoors.Length);


        if (newIndex != index)
        {
            index = newIndex;
            /*_onDoorIndex = (index) % 4;
            _onDoors[_onDoorIndex] = _allDoors[index];*/
            foreach (var door in _allDoors)
            {
                door.SetActive(false);
            }
            
            _allDoors[index].SetActive(true);
            _allDoors[(index+1)%_allDoors.Length].SetActive(true);
            _allDoors[(index+2)%_allDoors.Length].SetActive(true);
            _allDoors[(index+3)%_allDoors.Length].SetActive(true);


//            foreach (var door in _onDoors)
//            {
//                door.SetActive(true);
//            }
        }

 
    }
}