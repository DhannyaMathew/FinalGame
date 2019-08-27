using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float openSpeed;
    [SerializeField] private float openAngle;
    [SerializeField] private bool locked;
    [SerializeField] private float interactDistance = 1f;

    [SerializeField]private bool open;
    private int _direction;
    private Transform _hinge;

    // Start is called before the first frame update
    void Start()
    {
        _hinge = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!locked)
        {
            if (Input.GetButtonDown("Interact"))
            {
                if ((GameManager.PlayerTransform.position - transform.position).magnitude < interactDistance)
                {
                    open = !open;
                    if (open)
                    {
                        _direction =
                            (int) Mathf.Sign(Vector3.Dot(GameManager.PlayerTransform.position - transform.position,
                                transform.right));
                    }
                }
            }
        }

        _hinge.rotation = Quaternion.Lerp(_hinge.rotation, Quaternion.Euler(0, open ? _direction * openAngle : 0, 0),
            openSpeed * Time.deltaTime);
    }

    public void Lock()
    {
        Debug.Log("Locked");
        locked = true;
        open = false;
    }

    public void Unlock()
    {
        Debug.Log("Unlocked");
        locked = false;
    }
}