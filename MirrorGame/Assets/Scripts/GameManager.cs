
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public KeyCode RestartKey;
    public KeyCode QuitKey;

    public static Transform PlayerTransform { get; private set; }
    public static bool Paused { get; private set; }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        PlayerTransform = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Paused = !Paused;
            Cursor.visible = !Paused;
            Cursor.lockState = Paused ? CursorLockMode.Locked : CursorLockMode.None;
        }
        if (Input.GetKeyDown(QuitKey))
        {
            Quit();
        }
        if (Input.GetKeyDown(RestartKey))
        {
            RestartLevel();
        }
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

    public void Test()
    {
        Debug.Log("awe");
    }
}