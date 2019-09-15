using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable
{

    private bool _isHeld;

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnInteract()
    {
        Debug.Log("Awe");
    }
}
