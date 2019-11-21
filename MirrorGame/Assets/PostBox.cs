using System.Collections;
using System.Collections.Generic;
using PlayerManager;
using UnityEngine;

public class PostBox : Interactable
{
    private PostBoxDoor[] _doors;

    // Start is called before the first frame update
    void Start()
    {
        _doors = GetComponentsInChildren<PostBoxDoor>(true);
        playAnimation = false;
    }

    protected override void ResetObject()
    {
        foreach (var door in _doors)
        {
            door.Close();
        }
    }
    

    protected override void OnInteract()
    {
        var closest = 0f;
        PostBoxDoor closestDoor= null;
        foreach (var door in _doors)
        {
            var diff = (door.transform.position- GameManager.MainCamera.transform.position).normalized;
            var d = Vector3.Dot(GameManager.MainCamera.transform.forward,diff);
            if (d > closest)
            {
                closest = d;
                closestDoor = door;
            }
        }
        if (closestDoor != null)
        {
            closestDoor.Toggle();
        }
    }
}