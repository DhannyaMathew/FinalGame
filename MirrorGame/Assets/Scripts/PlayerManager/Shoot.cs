using MainMenu;
using UnityEngine;

namespace PlayerManager
{
    internal class Shoot : MonoBehaviour
    {
        private bool _hasMirror;
        private MirrorProjectile _mirrorProjectile;

        public bool HasMirror => _hasMirror;

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
                UiControl.ShowHintUI();
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
                GameManager.Player.PlayInteractAnim();
                UiControl.HideHintUI();
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(_mirrorProjectile.transform.position,
                    GameManager.MainCamera.transform.forward, out hit,
                    Mathf.Infinity))
                {
                    _hasMirror = false;
                    _mirrorProjectile.Shoot(hit);
                    //GameManager.CurrentLevel.ResetMirrors();
                }
            }
        }

        public void ResetObject()
        {
            _mirrorProjectile.gameObject.SetActive(false);
            _hasMirror = false;
        }

        public void PutBackMirror()
        {
            _hasMirror = _mirrorProjectile.isBig;
            _mirrorProjectile.Shrink();
        }
    }
}