using MainMenu;
using UnityEngine;

namespace PlayerManager
{
    internal class Shoot : MonoBehaviour
    {
        private MirrorProjectile _mirrorProjectile;

        public bool HasMirror => _mirrorProjectile.hasMirror;

        private void Awake()
        {
            _mirrorProjectile = GetComponentInChildren<MirrorProjectile>();
            _mirrorProjectile.Unload();
        }

        private void Start()
        {
            EventHandler.OnMirrorPickup += () =>
            {
                
                _mirrorProjectile.Load();
            };
            EventHandler.OnMirrorAbsorb += () => { _mirrorProjectile.Load(); };

            EventHandler.OnDoorWalkThrough += (door, transition) => { _mirrorProjectile.Unload(); };
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Fire1") && HasMirror)
            {
                RaycastHit hit;
                // GameManager.Player.PlayInteractAnim();
                UiControl.HideHintUI();
                if (Physics.Raycast(_mirrorProjectile.transform.position,
                    GameManager.MainCamera.transform.forward, out hit,
                    Mathf.Infinity))
                {
                    _mirrorProjectile.Shoot(hit);
                }
            }
        }

        public void ResetObject()
        {
            _mirrorProjectile.Unload();
        }

        public void PutBackMirror()
        {
            Debug.Log(HasMirror);
            _mirrorProjectile.Unload();
        }
    }
}