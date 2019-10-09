using UnityEngine;

namespace PlayerManager
{
    public class MainCamera : MonoBehaviour
    {
        [SerializeField] private float xSensitivity = 5;
        [SerializeField] private float ySensitivity = 5;

        private MainCameraSettings _settings;
        public float Theta { get; private set; }
        public float Phi { get; private set; }
        public Camera Camera { get; private set; }

        private Transform _target;
        private float _dist;
        private float _actualDist;

        private float _acutalTheta;
        private float _acutalPhi = -35;

        public Vector3 Target => _target.position + Vector3.up * 1.8f;

        private void Start()
        {
            _target = GameManager.Player.transform;
            Theta = _acutalTheta = _settings.startRotation.x;
            Phi = _acutalPhi = _settings.startRotation.y;
            Camera = GetComponent<Camera>();
        }

        //Should be in Settings Menu but was unsure if the fields should be made public - Dhannya
        public void SetCameraSensitivityX(float Xval)
        {
            //Camera Sensitivity X
            xSensitivity = Xval;
        }

        public void SetCameraSensitivityY(float Yval)
        {
            //Camera Sensitivity Y
            ySensitivity = Yval;
        }

        // Update is called once per frame
        void Update()
        {
            if (!GameManager.Paused)
            {
                var diff = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                Theta += diff.x * xSensitivity;
                Phi += diff.y * ySensitivity;
                Phi = Mathf.Clamp(Phi, _settings.minAngle, _settings.maxAngle);
                _acutalPhi = Mathf.Lerp(_acutalPhi, Phi, _settings.rotateLerpSpeed * Time.deltaTime);
                _acutalTheta = Mathf.Lerp(_acutalTheta, Theta, _settings.rotateLerpSpeed * Time.deltaTime);
                var offset = Quaternion.AngleAxis(_acutalTheta, Vector3.up) *
                             Quaternion.AngleAxis(-_acutalPhi, Vector3.right) *
                             Vector3.forward;

                var targetPos = Target;
                var ray = new Ray(targetPos, -offset);
                _dist = _settings.maxDist;
                if (_settings.alwaysShowPlayer)
                {
                    if (Physics.Raycast(ray, out var intersectCheck, _settings.maxDist))
                        _dist = intersectCheck.distance - 0.01f;
                }

                _actualDist = Mathf.Lerp(_actualDist, _dist, _settings.cameraDistLerpSpeed * Time.deltaTime);
                transform.position = targetPos - _actualDist * offset;
                transform.LookAt(targetPos);
            }
        }

        public void SetRotation(float theta, float phi)
        {
            Theta = _acutalTheta = theta;
            Phi = _acutalPhi = phi;
        }

        public void SetLevelSettings(Level level)
        {
            _settings = level.cameraSettings;
            
        }
    }
}