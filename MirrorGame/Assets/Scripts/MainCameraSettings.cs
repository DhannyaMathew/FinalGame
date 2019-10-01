using UnityEngine;

[CreateAssetMenu]
public class MainCameraSettings : ScriptableObject
{
    public float maxDist = 10;
    public float minAngle = -35f;
    public float maxAngle = 45f;
    public float rotateLerpSpeed = 1f;
    public float cameraDistLerpSpeed = 3f;
    public Vector2 startRotation;
    public bool alwaysShowPlayer;
}