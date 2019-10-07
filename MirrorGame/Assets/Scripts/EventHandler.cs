using System;

public static class EventHandler
{
    
    public delegate void DoorEvent(Door door);
    public delegate void MirrorEvent(Mirror mirror);
    public delegate void LevelTransitionEvent(Door door, Level.Transition transition);
    public delegate void KeyPickupEvent(Key key);
    public delegate void InteractEvent();
    public delegate void FallOutOfMapEvent();

    public static MirrorEvent OnMirrorWalkThrough;
    public static KeyPickupEvent OnKeyPickUp;
    public static DoorEvent OnDoorInteract;
    public static LevelTransitionEvent OnDoorWalkThrough;
    public static FallOutOfMapEvent OnFallOutOfMap;
    public static DoorEvent OnPortalTeleport;
    public static InteractEvent OnInteract;
    public static InteractEvent OnMirrorPickup;
    public static InteractEvent OnMirrorAbsorb;
}