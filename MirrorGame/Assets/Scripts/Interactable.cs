using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected float successAngle;
    [SerializeField] protected float successDistance;
    [SerializeField] protected bool debug;
    protected abstract void OnInteract();

    public static void Interact(Interactable[] interactables, Transform player)
    {
        var minDist = float.PositiveInfinity;
        var forward = player.forward;
        Interactable i = null;
        foreach (var interactable in interactables)
        {
            var ab = interactable.transform.position - player.position;
            if (ab.y >= 0 && ab.y < 1.8f)
            {
                forward = Vector3.Scale(forward, new Vector3(1, 0, 1));
                ab = Vector3.Scale(ab, new Vector3(1, 0, 1));
                var angle = Vector3.Angle(ab, forward);
                if (interactable.debug)
                {
                    Debug.Log(interactable.gameObject.name + ": " + angle);
                }

                if (angle < interactable.successAngle)
                {
                    if (interactable.debug)
                    {
                        Debug.Log(interactable.gameObject.name);
                    }
                    var dist = ab.magnitude;
                    if (dist < minDist && dist < interactable.successDistance)
                    {
                        minDist = dist;
                        i = interactable;
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