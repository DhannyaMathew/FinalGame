using UnityEngine;

public class Chain : MonoBehaviour
{
    [SerializeField] private float maxVelocity = 4;
    [SerializeField] private Vector2 minMaxDrag;
    [SerializeField] private AnimationCurve veloctiiyDrag;
    [SerializeField] private float maxAngularVelocity=4;
    [SerializeField] private Vector2 minMaxAngularDrag;
    [SerializeField] private AnimationCurve angularVeloctiiyAngularDrag;

    private AudioSource[] _soundsChain;
    private AudioSource _soundFalling;
    private void Start()
    {
  
        var dragLm = new LinearMapping{minMaxIn =  Vector2.up, minMaxOut = minMaxDrag};
        var angularDragLm = new LinearMapping{minMaxIn =  Vector2.up, minMaxOut = minMaxAngularDrag};
        foreach (var link in   GetComponentsInChildren<Link>())
        {
            link.SetPhysics(veloctiiyDrag, angularVeloctiiyAngularDrag, dragLm, angularDragLm, maxVelocity, maxAngularVelocity);   
        }
    }

    public void Disable()
    {
    }

    


}