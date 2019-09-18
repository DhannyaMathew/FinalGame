using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    
    [SerializeField] private KeyCode restartKey;
    [SerializeField] private KeyCode quitKey;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject mainCameraPrefab;
    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private Level[] _levels;
    
    private int _currentLevel = 0;
    private bool _paused = false;
    private Player _player;
    private MainCamera _mainCamera;
    private Orb _orb;
    
    public static bool Paused => _instance._paused;
    public static Level CurrentLevel => _instance._levels[_instance._currentLevel];
    public static Player Player => _instance._player;
    public static MainCamera MainCamera => _instance._mainCamera;
    public static Orb Orb => _instance._orb;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
        
        _player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
        _mainCamera = Instantiate(mainCameraPrefab, Vector3.zero, Quaternion.identity).GetComponent<MainCamera>();
        _orb = Instantiate(orbPrefab, Vector3.zero, Quaternion.identity).GetComponent<Orb>();
    }

    private void Start()
    {
        CurrentLevel.Setup(Player, MainCamera, Orb);
        LinkLevels();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _paused = !_paused;
            Cursor.visible = !Paused;
            Cursor.lockState = !Paused ? CursorLockMode.Locked : CursorLockMode.None;
        }
        if (Input.GetKeyDown(quitKey))
        {
            Quit();
        }
        if (Input.GetKeyDown(restartKey))
        {
            RestartLevel();
        }
    }

    private void LinkLevels()
    {
        for (var i = 0; i < _levels.Length-1; i++)
        {
            _levels[i].Link(_levels[i + 1]);
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
}