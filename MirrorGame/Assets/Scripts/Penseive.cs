using System;
using System.Collections.Generic;
using UnityEngine;

public class Penseive : Interactable
{
    [SerializeField] private int mirrors = 1;
    private int _initial;
    private GameObject _liquidMirror;

    private void Awake()
    {
        _initial = mirrors;
        _liquidMirror = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        CanBeInteractedWith = GameManager.Player.HasMirror || mirrors > 0;
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
                _liquidMirror.SetActive(false);
            }
        }
        else
        {
            if (GameManager.Player.HasMirror)
            {
                _liquidMirror.SetActive(true);
                mirrors++;
                GameManager.Player.PutBackMirror();
            }
        }
    }

    protected override void ResetObject()
    {
        mirrors = _initial;
        CanBeInteractedWith = mirrors != 0;
        _liquidMirror.SetActive(mirrors != 0);
    }
}