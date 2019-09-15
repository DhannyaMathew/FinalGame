using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class Player : MonoBehaviour
{
    [SerializeField] private MainCamera mainCamera;
    [SerializeField] private LayerMask interactables;
    [SerializeField] private float interactDistance = 1.5f;
    private PlayerMove Movement { get; set; }

    private void Awake()
    {
        Movement = GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Paused)
            Movement.Move(mainCamera.FlatDirection);

        var ray = mainCamera.Camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        ray.origin = mainCamera.Target - Vector3.up * 0.9f;
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * interactDistance, Color.yellow);
        if (Input.GetButtonDown("Interact"))
        {
            if (Physics.Raycast(ray, out var hit, interactDistance, interactables))
            {
                Debug.Log(hit.transform.gameObject.name);
                var interactable = hit.transform.gameObject.GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactable.OnInteract();
                }
            }
        }
    }


    public void Mirror()
    {
        mainCamera.Mirror();
    }
}