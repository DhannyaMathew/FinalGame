using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    
    public static bool Paused { get; private set; }

    private void Start()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Paused = !Paused;
            Cursor.visible = !Paused;
            Cursor.lockState = Paused ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}