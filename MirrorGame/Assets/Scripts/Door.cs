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

    private Quaternion rotation = Quaternion.identity;
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
                        rotation = _hinge.localRotation*Quaternion.Euler(0,-openAngle,0);
                    }
                    else
                    {
                        rotation = _hinge.localRotation*Quaternion.Euler(0,openAngle,0);
                    }
                }
                else
                {
                    _audioSource.PlayOneShot(lockSound);
                }
            }
        }
        
        _hinge.localRotation = Quaternion.Lerp(_hinge.localRotation, rotation, Time.deltaTime*openSpeed);
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