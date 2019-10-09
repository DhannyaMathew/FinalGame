using UnityEngine;

namespace PlayerManager
{
    [CreateAssetMenu]
    public class CharacterSettings : ScriptableObject
    {
        [SerializeField] internal float walkSpeed = 3f;
        [SerializeField] internal float runSpeed = 4.5f;
        [SerializeField] internal float climbSpeed = 2f;
        [SerializeField] internal float width = 1f;
        [SerializeField] internal float height = 1.81f;
        [SerializeField, Range(0, 1)] internal float bottomDetector = 0.1f;
        [SerializeField, Range(0, 1)] internal float topDetector = 0.4f;
        [SerializeField] internal float rotateSpeed = 15f;
        [SerializeField] internal float acceleration = 10f;
        [SerializeField, Range(0, 180)] internal float maxGroundAngle;
        [SerializeField] internal LayerMask ground;
        [SerializeField] internal LayerMask ladder;
        [SerializeField] internal float walkSoundScale = 0.1f;
        [SerializeField] internal bool debug;
    }
}