using UnityEngine;

namespace PlayerManager
{
    [CreateAssetMenu]
    public class MainCameraSettings : ScriptableObject
    {
        [SerializeField]internal float maxDist = 10;
        [SerializeField]internal float minAngle = -35f;
        [SerializeField]internal float maxAngle = 45f;
        [SerializeField]internal float rotateLerpSpeed = 1f;
        [SerializeField]internal float cameraDistLerpSpeed = 3f;
        [SerializeField]internal Vector2 startRotation;
        [SerializeField]internal bool alwaysShowPlayer;
    }
}