public class EventHandler
{
    public delegate void DoorEvent(Door door);

    public delegate void MirrorEvent(Mirror mirror);

    public delegate void LevelTransitionEvent(GameManager.LevelTransition transition);

    public delegate void KeyPickupEvent(Key key);

    public static DoorEvent OnDoorInteract;
    public static MirrorEvent OnMirrorWalkThrough;
    public static LevelTransitionEvent OnLevelChange;
    public static KeyPickupEvent OnKeyPickUp;
}