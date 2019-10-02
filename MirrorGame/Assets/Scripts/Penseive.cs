using System.Collections.Generic;
using UnityEngine;

public class Penseive : Interactable
{
    [SerializeField] private int mirrors = 1;
    private GameObject _liquidMirror;

    private void Awake()
    {
        _liquidMirror = transform.GetChild(0).gameObject;
    }


    protected override void OnInteract()
    {
        if (mirrors > 0)
        {
            mirrors--;
            if (EventHandler.OnMirrorPickup != null)
                EventHandler.OnMirrorPickup();
            if (mirrors == 0)
            {
                CanBeInteractedWith = false;
                _liquidMirror.SetActive(false);
            }
        }
    }

    protected override void ResetObject()
    {
    }
}