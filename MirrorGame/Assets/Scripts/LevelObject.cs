using UnityEngine;

public abstract class LevelObject : MonoBehaviour
{
    public Level Level { get; protected set; }

    protected virtual void Start()
    {
        Level = GetComponentInParent<Level>();
    }

    protected abstract void ResetObject();
}