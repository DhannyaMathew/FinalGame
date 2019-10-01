
using UnityEngine;

public abstract class Pickupable:Interactable
{
    protected bool _isHeld;
    [SerializeField] protected bool allowPickup = true;

    protected override void OnInteract()
    {
        if (allowPickup)
        {
            if (!_isHeld)
            {
                _isHeld = true;
                OnPickup();
            }
        }
    }

    protected abstract void OnPickup();
}