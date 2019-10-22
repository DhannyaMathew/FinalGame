
using UnityEngine;

public abstract class Pickupable:Interactable
{
    protected bool _isHeld;

    protected override void OnInteract()
    {
       
            if (!_isHeld)
            {
                _isHeld = true;
                OnPickup();
            }
        
    }

    
    protected abstract void OnPickup();
}