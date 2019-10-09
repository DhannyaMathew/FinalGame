using UnityEngine;

namespace PlayerManager
{
    internal class Shoot : MonoBehaviour
    {
        private bool _hasMirror;
        private MirrorProjectile _mirrorProjectile;

        private void Awake()
        {
            _mirrorProjectile = GetComponentInChildren<MirrorProjectile>();
            _mirrorProjectile.gameObject.SetActive(false);
        }

        private void Start()
        {
            EventHandler.OnMirrorPickup += () =>
            {
                _mirrorProjectile.Load();
                _hasMirror = true;
            };
            EventHandler.OnMirrorAbsorb += () =>
            {
                _mirrorProjectile.gameObject.SetActive(true);
                _mirrorProjectile.Grow();
                _hasMirror = true;
            };
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Fire1") && _hasMirror)
            {
                RaycastHit hit;

                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(_mirrorProjectile.transform.position,
                    GameManager.MainCamera.transform.forward, out hit,
                    Mathf.Infinity))
                {
                    _hasMirror = false;
                
                    _mirrorProjectile.Shoot(hit.point, hit.normal, GameManager.MainCamera.transform.forward);
                    //GameManager.CurrentLevel.ResetMirrors();
                }
            }
        }
    }
}