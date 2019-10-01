using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected float successAngle;
    [SerializeField] protected float successDistance;
    protected abstract void OnInteract();

    public static void Interact(IEnumerable<Interactable> interactables, Transform player)
    {
        var minDist = float.PositiveInfinity;
        var forward = player.forward;
        Interactable i = null;
        foreach (var interactable in interactables)
        {
            if (interactable.gameObject.activeInHierarchy)
            {
                var ab = interactable.transform.position - player.position;
                if (ab.y >= -0.3f && ab.y < 1.8f)
                {
                    forward = Vector3.Scale(forward, new Vector3(1, 0, 1));
                    ab = Vector3.Scale(ab, new Vector3(1, 0, 1));
                    var angle = Vector3.Angle(ab, forward);
                    if (angle < interactable.successAngle)
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
            i.OnInteract();
            if (EventHandler.OnInteract != null)
                EventHandler.OnInteract();
        }
    }
}