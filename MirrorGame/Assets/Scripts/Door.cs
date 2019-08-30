using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float openSpeed;
    [SerializeField] private float openAngle;
    [SerializeField] private bool locked;
    [SerializeField] private float interactDistance = 1f;
    [SerializeField] private bool open;
    [SerializeField] private AudioClip lockSound;
    [SerializeField] private AudioClip openSound;

    private int _direction;
    private Transform _hinge;

    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        _hinge = transform.GetChild(0);
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if ((GameManager.PlayerTransform.position - transform.position).magnitude < interactDistance)
            {
                if (!locked)
                {
                    _audioSource.PlayOneShot(openSound);
                    open = !open;
                    if (open)
                    {
                        _direction =
                            (int) Mathf.Sign(Vector3.Dot(GameManager.PlayerTransform.position - transform.position,
                                transform.right));
                    }
                }
                else
                {
                    _audioSource.PlayOneShot(lockSound);
                }
            }
        }

        _hinge.rotation = Quaternion.Lerp(_hinge.rotation, Quaternion.Euler(0, open ? -_direction * openAngle : 0, 0),
            openSpeed * Time.deltaTime);
    }

    public void Lock()
    {
        locked = true;
        open = false;
    }

    public void Unlock()
    {
        locked = false;
    }
}