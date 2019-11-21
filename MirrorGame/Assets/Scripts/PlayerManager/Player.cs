using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerManager
{
    [RequireComponent(
        typeof(Shoot),
        typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private CharacterSettings settings;

        private MainCamera _mainCamera;
        private Rigidbody _rigidbody;
        private PlayerMove _movement;
        private Shoot _shoot;
        private Animator _animator;
        private List<ContactPoint> _collisions;
        private List<ContactPoint> _collCollect;
        private Timer _footStepModulator;
        private AudioSource _audioSource;
        private GameObject _tempKey;
        private ParticleSystem _ps;

        private static readonly int SpeedAnimatorParameter = Animator.StringToHash("Speed");
        private static readonly int YDirAnimatorParameter = Animator.StringToHash("yDir");
        private static readonly int XDirAnimatorParameter = Animator.StringToHash("xDir");
        private static readonly int OnLadder = Animator.StringToHash("OnLadder");
        private static readonly int Falling = Animator.StringToHash("Falling");
        private static readonly int ActualSpeed = Animator.StringToHash("ActualSpeed");
        private static readonly int Interact = Animator.StringToHash("Interact");
        public bool HasMirror => _shoot.HasMirror;
        private bool _interacting;

        private void Awake()
        {
            _collisions = new List<ContactPoint>();
            _collCollect = new List<ContactPoint>();
            _shoot = GetComponent<Shoot>();
            _animator = GetComponentInChildren<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _audioSource = GetComponent<AudioSource>();
            _movement = new PlayerMove(settings, _rigidbody);
            _ps = GetComponentInChildren<ParticleSystem>(true);
            _footStepModulator = new Timer(1f, true, () =>
            {
                var mag = _movement.HasInput ? _movement.Speed : 0;
                float s;
                if (mag > 0.01f)
                {
                    s = 1f / mag;
                }
                else
                {
                    if (_audioSource.isPlaying)
                    {
                        _audioSource.Stop();
                    }

                    return;
                }

                if (_audioSource.isPlaying)
                {
                    _audioSource.time = 0f;
                }

                _audioSource.Play();
                _footStepModulator.Length = s * settings.walkSoundScale;
            });
            _tempKey = transform.GetChild(1).gameObject;
            EventHandler.OnKeyPickUp += (key) =>
            {
                
                key.gameObject.SetActive(false);
                _tempKey.SetActive(true);
            };
            EventHandler.OnDoorInteract += door =>
            {
                if (_tempKey.activeSelf)
                {
                    if (door.IsLocked)
                        _tempKey.SetActive(false);
                    door.Unlock();
                }
            };
            EventHandler.OnDoorWalkThrough += (door, transition) => { _tempKey.SetActive(false); };
        }

        public void Setup(MainCamera mainCamera)
        {
            ResetObjects();
            _mainCamera = mainCamera;
        }

        private void Update()
        {
            _movement.GetInput();
            Interactable.Interact(GameManager.CurrentLevel.Interactables, transform, settings.height);
            UpdateAnimator();
            _footStepModulator.Tick(Time.deltaTime);
        }

        internal void OnDrawGizmos()
        {
            if (settings.debug)
            {
                _movement?.DrawDebug();
            }
        }

        private void UpdateAnimator()
        {
            _animator.SetFloat(SpeedAnimatorParameter, _movement.Speed / 2f);
            _animator.SetFloat(XDirAnimatorParameter, _movement.MoveDirection.x);
            _animator.SetFloat(YDirAnimatorParameter, _movement.MoveDirection.z);
            _animator.SetBool(OnLadder, _movement.OnLadder);
            _animator.SetBool(Falling, _movement.Falling);
            _animator.SetFloat(ActualSpeed, _movement.ActualSpeed);
        }

        private void FixedUpdate()
        {
            CollectContacts();


            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
            {
                _interacting = false;
            }

            _movement.Move(_mainCamera.Theta, _collisions, _interacting);
            if (GameManager.CurrentLevelIndex < 11)
            {
                if (transform.position.y < -50f)
                {
                    FallOffMap();
                }
            }
            else
            {
                if (transform.position.y < -50f)
                {
                    FallToMain();
                }
            }

            _collisions.Clear();
        }

        private void FallToMain()
        {
            GameManager.FullReload();
        }

        private void CollectContacts()
        {
            var reduced = new List<ContactPoint>();
            for (var i = 0; i < _collCollect.Count; i++)
            {
                if (i == 0) reduced.Add(_collCollect[i]);
                else if (_collCollect[i - 1].point != _collCollect[i].point) reduced.Add(_collCollect[i]);
            }

            _collisions = reduced;
            _collCollect.Clear();
        }

        public void Teleport(Transform teleporter, Transform target)
        {
            var angleDiff = target.eulerAngles - teleporter.eulerAngles;
            angleDiff.y += 180;
            _mainCamera.SetRotation(_mainCamera.Theta + angleDiff.y, _mainCamera.Phi + angleDiff.x);
            transform.Rotate(angleDiff);
            transform.position = target.position;
        }

        private void FallOffMap()
        {
            GameManager.RestartLevel();
            _rigidbody.velocity = new Vector3(0, 0, 0);
        }


        private void OnCollisionStay(Collision other)
        {
            foreach (var contact in other.contacts)
            {
                _collCollect.Add(contact);
            }
        }

        public void ResetObjects()
        {
            _shoot.ResetObject();
            _tempKey.SetActive(false);
        }

        public void PutBackMirror()
        {
            _shoot.PutBackMirror();
        }

        public void PlayInteractAnim()
        {
            _animator.SetTrigger(Interact);
            _interacting = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Puddle"))
                _ps.Play();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Puddle"))
                _ps.Stop();
        }
    }
}