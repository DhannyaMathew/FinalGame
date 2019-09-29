public class EventHandler
{
    public delegate void DoorEvent(Door door);
    public delegate void MirrorEvent(Mirror mirror);
    public delegate void LevelTransitionEvent(Level.Transition transition);
    public delegate void KeyPickupEvent(Key key);
    public delegate void InteractEvent();

    public static MirrorEvent OnMirrorWalkThrough;
    public static KeyPickupEvent OnKeyPickUp;
    public static DoorEvent OnDoorInteract;
    public static LevelTransitionEvent OnDoorWalkThrough;
    public static InteractEvent OnInteract;
}