using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class Player : MonoBehaviour
{
    [SerializeField] private MainCamera mainCamera;
    private PlayerMove Movement { get; set; }

    private void Awake()
    {
        Movement = GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Paused)
            Movement.GetInput();
    }

    private void FixedUpdate()
    {
        if (!GameManager.Paused)
            Movement.Move(mainCamera.FlatDirection);
    }
}