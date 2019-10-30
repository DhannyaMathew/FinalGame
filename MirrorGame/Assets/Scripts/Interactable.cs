using System;
using System.Collections.Generic;
using MainMenu;
using UnityEngine;

public abstract class Interactable : LevelObject
{
    [SerializeField] protected float successAngle;
    [SerializeField] protected float successDistance;

    public bool CanBeInteractedWith;

    protected abstract void OnInteract();

    public static void Interact(IEnumerable<Interactable> interactables, Transform player, float height)
    {
        var minDist = float.PositiveInfinity;
        var forward = Vector3.Scale(player.forward, new Vector3(1, 0, 1));
        Interactable i = null;
        foreach (var interactable in interactables)
        {
            if (interactable.gameObject.activeInHierarchy && interactable.CanBeInteractedWith)
            {
                if ((player.position - interactable.transform.position).sqrMagnitude <
                    interactable.successDistance * interactable.successDistance)
                {
                    var r = Vector3.Scale(interactable.transform.position - player.position + Vector3.up * 0.905f,
                        new Vector3(1, 0, 1));

                    var angle = Vector3.Angle(r, forward);

                    if (angle < interactable.successAngle)
                    {
                        var dist = r.magnitude;
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
                GameManager.Player.PlayInteractAnim();
                i.OnInteract();
                if (EventHandler.OnInteract != null)
                    EventHandler.OnInteract();
            }
            else
            {
                UiControl.ShowInteractUI();
            }
        }
        else
        {
            UiControl.HideInteractUI();
        }
    }


    private static bool IsInCapsule(Vector3 origin, Vector3 normal, float height, float radius, Vector3 point)
    {
        var p = Vector3.Dot(point, normal.normalized);
        Debug.DrawLine(origin, origin + p * normal, Color.green);
        if (!(p >= -radius) || !(p <= height + radius)) return false;
        var o = origin + p * normal;
        return (point - o).sqrMagnitude < radius * radius;
    }
}