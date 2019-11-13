
using UnityEngine;

public abstract class Pickupable: Interactable
{
    protected bool IsHeld;

    protected override void OnInteract()
    {
       
            if (!IsHeld)
            {
                IsHeld = true;
                OnPickup();
            }
        
    }

    
    protected abstract void OnPickup();
}