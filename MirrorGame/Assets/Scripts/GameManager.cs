
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    [SerializeField] private KeyCode RestartKey;
    [SerializeField] private KeyCode QuitKey;
    [SerializeField] private GameObject orb;
    public static bool Paused { get; private set; }
    public static MainCamera MainCamera { get; private set; }
    public static Player Player { get; private set; }

    private Mirror[] _mirrors;
    private bool _turnMirrorsBackOn = false;
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        Player = FindObjectOfType<Player>();
        MainCamera = FindObjectOfType<MainCamera>();
    }

    private void Start()
    {
        _mirrors = FindObjectsOfType<Mirror>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Paused = !Paused;
            Cursor.visible = !Paused;
            Cursor.lockState = !Paused ? CursorLockMode.Locked : CursorLockMode.None;
        }
        if (Input.GetKeyDown(QuitKey))
        {
            Quit();
        }
        if (Input.GetKeyDown(RestartKey))
        {
            RestartLevel();
        }

        if (_instance._turnMirrorsBackOn)
            TurnOnMirrors();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void TurnOffMirrors()
    {
        foreach (var mirror in _instance._mirrors)
        {
            mirror.gameObject.SetActive(false);
        }

        _instance._turnMirrorsBackOn = true;
    }
    
    public static void TurnOnMirrors()
    {
        foreach (var mirror in _instance._mirrors)
        {
            mirror.gameObject.SetActive(true);
        }

        _instance._turnMirrorsBackOn = false;
    }
}