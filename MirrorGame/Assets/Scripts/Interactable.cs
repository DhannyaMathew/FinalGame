using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : LevelObject
{
    [SerializeField] protected float successAngle;
    [SerializeField] protected float successDistance;

    public bool CanBeInteractedWith;

    protected override void Start()
    {
        base.Start();
        CanBeInteractedWith = true;
    }

    protected abstract void OnInteract();

    public static void Interact(IEnumerable<Interactable> interactables, Transform player)
    {
        var minDist = float.PositiveInfinity;
        var forward = player.forward;
        Interactable i = null;
        foreach (var interactable in interactables)
        {
            if (interactable.gameObject.activeInHierarchy && interactable.CanBeInteractedWith)
            {
                var ab = interactable.transform.position - player.position;
                if (ab.y >= -0.3f && ab.y < 1.8f)
                {
                    forward = Vector3.Scale(forward, new Vector3(1, 0, 1));
                    ab = Vector3.Scale(ab, new Vector3(1, 0, 1));
                    var angle = Vector3.Angle(ab, forward);
                    var angle1 = Vector3.Angle(ab, -forward);
                    if (angle < interactable.successAngle || angle1 < interactable.successAngle)
                    {
                        var dist = ab.magnitude;
                        if (dist < minDist && dist < interactable.successDistance)
                        {
                            minDist = dist;
                            i = interactable;
                        }
                    }
                }
            }
        }

        if (i != null)
        {
            if (Input.GetButtonDown("Interact"))
            {
                i.OnInteract();
                if (EventHandler.OnInteract != null)
                    EventHandler.OnInteract();
            }
            else
            {
                GameManager.ShowInteractUI();
            }
        }
        else
        {
            GameManager.HideInteractUI();
        }
    }
}