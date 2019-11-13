using System.Collections.Generic;
using UnityEngine;

public abstract class LevelObject : MonoBehaviour
{
    public Level Level { get; protected set; }

    protected virtual void Start()
    {
        Level = GetComponentInParent<Level>();
    }

    protected abstract void  ResetObject();

    public static void ResetLevelObjects(IEnumerable<LevelObject> levelObjects)
    {
        foreach (var o in levelObjects)
        {
            Debug.Log(o+" reset");
            o.ResetObject();
        }
    }
}