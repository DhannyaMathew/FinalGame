
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;


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
    }

    public void Test()
    {
        Debug.Log("awe");
    }
}